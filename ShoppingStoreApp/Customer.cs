using ShoppingStoreApp.DataAccess.Models;
using ShoppingStoreApp.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingStoreApp
{
    public class Customer
    {
        public static async Task SignIn(ShoppingStoreModel shoppingStore)
        {
            Console.Clear();
            Console.WriteLine("-----------------------------------");
            Console.WriteLine($"*** Welcome to {shoppingStore.Name}! Please sign in by entering your username and password ***");
            Console.WriteLine("-----------------------------------");
            Console.Write("Enter your Username: ");
            string username = Console.ReadLine().ToLower();
            Console.Write("Enter your Password: ");
            string password = Console.ReadLine().ToLower();

            var customers = await GetAllCustomers(); //hämtar ut alla customers från text filen

            var customer = customers.FirstOrDefault(x => x.Username == username && x.Password == password);

            //checkCustomerExists: kollar om kunden finns och användaren har skrivit in rätt användarnamn och lösenord
            var checkCustomerExists = customers.Any(x => x.Username == username && x.Password == password);
            //checkCustomerIncorrectPassword: kollar om användaren skrivit in rätt namn men fel lösenord
            var checkCustomerIncorrectPassword = customers.Any(x => x.Username == username && x.Password != password);
            //cehckCustomerNotExist: Kollar om kontot finns eller inte
            var checkCustomerNotExists = customer == null;

            if (checkCustomerExists)
            {
                await ShoppingStoreProcessor.ShoppingStoreMain(shoppingStore, customer);
            }

            if (checkCustomerIncorrectPassword)
            {
                while (true)
                {
                    Console.Clear();
                    Console.WriteLine("-----------------------------------");
                    Console.WriteLine("*** Sign in page ***");
                    Console.WriteLine("-----------------------------------");
                    Console.WriteLine("\nIncorrect Password. Do you want to Try Again? [y/n]\n");
                    Console.Write("\nSelect an option: ");

                    var responese = Console.ReadKey().Key;
                    if (responese == ConsoleKey.Y)
                    {
                        await SignIn(shoppingStore);
                        break;
                    }

                    if (responese == ConsoleKey.N)
                    {
                        ShoppingStoreDataAccess db = new ShoppingStoreDataAccess();
                        var shoppingStores = await db.GetAllShoppingStore();
                        await Program.ChooseShoppingStoreMainMenu(shoppingStores);
                        break;
                    }
                }
            }

            if (checkCustomerNotExists)
            {
                while (true)
                {
                    Console.Clear();
                    Console.WriteLine("-----------------------------------");
                    Console.WriteLine("*** Sign in page ***");
                    Console.WriteLine("-----------------------------------");
                    Console.WriteLine($"There is no customer registrered with name: {username}. Do you want to register a new customer? [y/n]\n");
                    Console.Write("\nSelect an option: ");

                    var responese = Console.ReadKey().Key;
                    if (responese == ConsoleKey.Y)
                    {
                        await ShoppingStoreProcessor.RegisterNewCustomer(shoppingStore);
                        break;
                    }

                    if (responese == ConsoleKey.N)
                    {
                        ShoppingStoreDataAccess db = new ShoppingStoreDataAccess();
                        var shoppingStores = await db.GetAllShoppingStore();
                        await Program.ChooseShoppingStoreMainMenu(shoppingStores);
                        break;
                    }

                }
            }
        }

        public static async Task SaveCustomer(CustomerModel customer)
        {
            ShoppingStoreDataAccess db = new ShoppingStoreDataAccess();
            await db.CreateCustomer(customer);
        }

        public static async Task SaveProductToCustomerCart(CustomerModel customer)
        {
            ShoppingStoreDataAccess db = new ShoppingStoreDataAccess();
            await db.UpdateCustomer(customer);
        }

        public static async Task DeleteAllProductFromCustomerCart(CustomerModel customer)
        {
            ShoppingStoreDataAccess db = new ShoppingStoreDataAccess();
            customer.ShoppingCart = new List<ProductModel>();
            await db.UpdateCustomer(customer);
        }

        public static async Task<List<CustomerModel>> GetAllCustomers()//retunerar en lista av customers
        {
            ShoppingStoreDataAccess db = new ShoppingStoreDataAccess();
            var customers = await db.GetAllCustomer();
            return customers;
        }

        public static async Task<CustomerModel> GetCustomer(string CustomerName)// retunerar en customer beroende på username som skickas in i metoden 
        {
            ShoppingStoreDataAccess db = new ShoppingStoreDataAccess();
            var customer = await db.GetCustomerByName(CustomerName);
            return customer;
        }

        public static async Task CreateInitialCustomers()
        {
            var customers = await GetAllCustomers();
            if (customers.Count > 0)
            {
                return;
            }

            var newCustomers = new List<CustomerModel>
            {
                new CustomerModel("knatte", "123", "123456789", "Drottninggatan 123", "Göteborg", "411 14", Enums.CustomerLevelType.Gold, Enums.CurrencyType.GBP, new List<ProductModel>()),
                new CustomerModel("fnatte", "321", "123456789", "Drottninggatan 321", "Göteborg", "411 14", Enums.CustomerLevelType.Silver, Enums.CurrencyType.SEK, new List<ProductModel>()),
                new CustomerModel("tjatte", "213", "123456789", "Drottninggatan 213", "Göteborg", "411 14",Enums.CustomerLevelType.Bronze, Enums.CurrencyType.USD, new List<ProductModel>()),
            };

            ShoppingStoreDataAccess db = new ShoppingStoreDataAccess();
            foreach (var customer in newCustomers)
            {
                await db.CreateCustomer(customer);
            }
        }
    }
}
