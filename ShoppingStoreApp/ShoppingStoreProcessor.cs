using ShoppingStoreApp.DataAccess.Models;
using ShoppingStoreApp.DataAccess;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingStoreApp
{
    public class ShoppingStoreProcessor
    {

        public static async Task ShoppingStoreMain(ShoppingStoreModel shoppingStore, CustomerModel customer)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("-----------------------------------");
                Console.WriteLine($"*** {shoppingStore.Name} Shopping store main menu ***");
                Console.WriteLine($"current user: {customer.Username}");
                Console.WriteLine($"current currency: {customer.CurrencyType}");
                Console.WriteLine("-----------------------------------");
                Console.WriteLine("1) Go to shopping");
                Console.WriteLine("2) Show shopping cart");
                Console.WriteLine("3) Check out");
                Console.WriteLine("4) Sign out");

                Console.Write("\nSelect an option to buy: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        await StartShopping(shoppingStore, customer);
                        break;
                    case "2":
                        await ShowShoppingCart(shoppingStore, customer);
                        break;
                    case "3":
                        await CheckOut(shoppingStore, customer);
                        break;
                    case "4":
                        var shoppingStores = await ShoppingStore.GetAllShoppingStores();
                        await Program.ChooseShoppingStoreMainMenu(shoppingStores);
                        break;

                }
            }

        }

        public static async Task RegisterNewCustomer(ShoppingStoreModel shoppingStore)//metod för att registrera ny kund
        {
            Console.Clear();
            Console.WriteLine("-----------------------------------");
            Console.WriteLine("***Register a new customer***");
            Console.WriteLine("-----------------------------------");

            Console.Write("Please enter Username: ");
            string username = Console.ReadLine();

            Console.Write("Please enter Password: ");
            string password = Console.ReadLine();

            Console.Write("Please enter Phone Number: ");
            string phoneNumber = Console.ReadLine();

            Console.Write("Please enter Address: ");
            string address = Console.ReadLine();

            Console.Write("Please enter City: ");
            string city = Console.ReadLine();

            Console.Write("Please enter Postal Code: ");
            string postalCode = Console.ReadLine();

            Console.Write("Please enter customer level (Default, Bronze, Silver or Gold): ");
            string inputCustomerType = Console.ReadLine().ToLower();

            var customerType = Enums.CustomerLevelType.Default;//deklarer en enum av customer type default
            switch (inputCustomerType)//baserat på user input sätter vi variabel customerType till den valda customerType
            {
                case "bronze":
                    customerType = Enums.CustomerLevelType.Bronze;
                    break;
                case "silver":
                    customerType = Enums.CustomerLevelType.Silver;
                    break;
                case "gold":
                    customerType = Enums.CustomerLevelType.Gold;
                    break;
                default:
                    customerType = Enums.CustomerLevelType.Default;
                    break;
            }

            Console.Write("Please enter customer currency (gbp, usd or sek): ");
            string inputCurrencyType = Console.ReadLine().ToLower();

            var currencyType = Enums.CurrencyType.SEK;
            switch (inputCurrencyType)
            {
                case "gbp":
                    currencyType = Enums.CurrencyType.GBP;
                    break;
                case "usd":
                    currencyType = Enums.CurrencyType.USD;
                    break;
                default:
                    currencyType = Enums.CurrencyType.SEK;
                    break;
            }

            CustomerModel newCustomer = new CustomerModel(username, password, phoneNumber, address, city, postalCode, customerType, currencyType, new List<ProductModel>());//ny instans av customer objekt baserat på user input
            Customer.SaveCustomer(newCustomer);

            Console.WriteLine();
            Console.WriteLine("Congratulations! You are now a customer to us. Press any key to continue");
            Console.ReadKey();

            var customer = await Customer.GetCustomer(newCustomer.Username);
            await ShoppingStoreMain(shoppingStore, customer);
        }

        public static async Task StartShopping(ShoppingStoreModel shoppingStore, CustomerModel customer)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("-----------------------------------");
                Console.WriteLine("*** Shopping page ***");
                Console.WriteLine("-----------------------------------");
                Console.WriteLine("Add product to cart by pressing the keys");


                var products = shoppingStore.Products;
                var counter = 1;
                var dictionary = new Dictionary<int, ProductModel>();
                foreach (var product in products)
                {
                    dictionary.Add(counter, product);
                    counter++;
                }

                foreach (var product in dictionary)
                {
                    Console.WriteLine($"{product.Key}. {product.Value.Name} {GetPriceAndConvertCurrency(product.Value.Price, customer.CurrencyType)} x {product.Value.StockBalance}");
                }
                Console.WriteLine("\nSelect an option. Enter 0 to go back to shopping main menu");
                Console.WriteLine("-----------------------------------");

                int userInputShoppingStore;
                while (!int.TryParse(Console.ReadLine(), out userInputShoppingStore))
                    Console.WriteLine("You entered an invalid key, please enter a valid key");

                if (userInputShoppingStore == 0)
                {
                    await ShoppingStoreMain(shoppingStore, customer);
                }

                foreach (var product in dictionary)
                {
                    if (userInputShoppingStore == product.Key)
                    {
                        customer.ShoppingCart?.Add(product.Value);

                        //WIP REMOVE PRODUCT
                        var productToRemove = shoppingStore.Products.Find(x => x.Id == product.Value.Id);


                        await Customer.SaveProductToCustomerCart(customer);
                        Console.WriteLine($"{product.Value.Name} is added to your shopping cart! Press any key to continue shopping!");
                        Console.ReadKey();
                    }
                }
            }
        }

        public static async Task ShowShoppingCart(ShoppingStoreModel shoppingStore, CustomerModel customer)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("-----------------------------------");
                Console.WriteLine($"*** Shopping cart for {customer.Username} with customer level {customer.CustomerLevelType} ***");
                Console.WriteLine("-----------------------------------");

                var totalPrice = customer.ShoppingCart.Sum(x => x.GetPrice(customer));

                var groupedProducts = customer.ShoppingCart.GroupBy(x => x.Name).Select(group => new
                {
                    ProductName = group.Key,
                    Price = group.Sum(x => x.GetPrice(customer)),
                    Count = group.Count(),
                });

                foreach (var groupedProduct in groupedProducts)
                {
                    Console.WriteLine($"{groupedProduct.Count}x {groupedProduct.ProductName} {GetPriceAndConvertCurrency(groupedProduct.Price, customer.CurrencyType)}");
                }

                Console.WriteLine();
                Console.WriteLine($"Total price: {GetPriceAndConvertCurrency(totalPrice, customer.CurrencyType)}");
                Console.WriteLine("\nEnter 0 to go back to shopping main menu");
                Console.WriteLine("-----------------------------------");
                int userInputShoppingStore;

                while (!int.TryParse(Console.ReadLine(), out userInputShoppingStore))
                    Console.WriteLine("You entered an invalid key, please enter a valid key");

                if (userInputShoppingStore == 0)
                {
                    await ShoppingStoreMain(shoppingStore, customer);
                }
            }

        }

        public static async Task CheckOut(ShoppingStoreModel shoppingStore, CustomerModel customer)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("-----------------------------------");
                Console.WriteLine("*** Checkout ***");
                Console.WriteLine("-----------------------------------");
                Console.WriteLine($"Are you sure want to Checkout? [y/n]\n");
                Console.Write("\nSelect an option: ");

                var responese = Console.ReadKey().Key;
                if (responese == ConsoleKey.Y)
                {
                    Console.WriteLine("\n\nChecking out successfully completed, here is your order details. See you next time!\n");
                    Console.WriteLine("-----------------------------------");
                    var orderDetails = await CreateOrderDetails(shoppingStore, customer);
                    Console.WriteLine($"Ordernumber: {orderDetails.Id}");
                    Console.WriteLine($"Order date: {orderDetails.OrderDate}");
                    Console.WriteLine($"Account Name: {orderDetails.CustomerName}");
                    Console.WriteLine($"Delivery Address: {orderDetails.DeliveryAddress}");
                    Console.WriteLine($"City: {orderDetails.City}");
                    Console.WriteLine($"Postal Code: {orderDetails.PostalCode}");
                    Console.WriteLine($"Ordered from store: {orderDetails.ShoppingStoreName}");
                    Console.WriteLine("-----------------------------------");
                    Console.WriteLine("Products:");
                    var totalPrice = orderDetails.Products.Sum(x => x.GetPrice(customer));

                    var groupedProducts = orderDetails.Products.GroupBy(x => x.Name).Select(group => new
                    {
                        ProductName = group.Key,
                        Price = group.Sum(x => x.GetPrice(customer)),
                        Count = group.Count(),
                    });

                    foreach (var groupedProduct in groupedProducts)
                    {
                        Console.WriteLine($"{groupedProduct.Count}x {groupedProduct.ProductName} {GetPriceAndConvertCurrency(groupedProduct.Price, customer.CurrencyType)}");
                    }

                    Console.WriteLine();
                    Console.WriteLine($"Total price: {GetPriceAndConvertCurrency(totalPrice, customer.CurrencyType)}");

                    await Customer.DeleteAllProductFromCustomerCart(customer);
                    Console.WriteLine("\nPress any key to Main Menu");
                    Console.ReadKey();
                    await ShoppingStoreMain(shoppingStore, customer);
                    break;
                }

                if (responese == ConsoleKey.N)
                {
                    await ShoppingStoreMain(shoppingStore, customer);
                    break;
                }
            }
        }

        public static async Task<OrderDetailsModel> CreateOrderDetails(ShoppingStoreModel shoppingStore, CustomerModel customer)
        {
            var orderdetails = new OrderDetailsModel
            {
                CustomerName = customer.Username,
                OrderDate = DateTime.Now,
                DeliveryAddress = customer.Address,
                City = customer.City,
                PostalCode = customer.PostalCode,
                DeliveryDate = null,
                ShoppingStoreName = shoppingStore.Name,
                Products = customer.ShoppingCart ?? new List<ProductModel>()
            };

            ShoppingStoreDataAccess db = new ShoppingStoreDataAccess();
            await db.CreateOrderDetails(orderdetails);
            return orderdetails;
        }


        private static string GetPriceAndConvertCurrency(decimal price, Enums.CurrencyType currency)
        {
            var cultureInfoString = "";
            switch (currency)
            {
                case Enums.CurrencyType.USD:
                    cultureInfoString = (price / 8).ToString("C", new CultureInfo("en-US")); //sätter pris och culturinfo baserat på enums currency type
                    break;
                case Enums.CurrencyType.GBP:
                    cultureInfoString = (price / 10).ToString("C", new CultureInfo("en-GB"));
                    break;
                default:
                    cultureInfoString = price.ToString("C", new CultureInfo("sv-SE"));
                    break;
            }

            return cultureInfoString;
        }
    }
}

