using ShoppingStoreApp.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingStoreApp
{
    public class ShoppingStoreManagement
    {
        public static async Task ShoppingStoreManagementSignIn()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("-----------------------------------");
                Console.WriteLine("*** Shopping store Management page ***");
                Console.WriteLine("-----------------------------------");
                Console.WriteLine("Choose shopping store by pressing the keys");

                var shoppingStores = await ShoppingStore.GetAllShoppingStores();

                var counter = 1;
                var dictionary = new Dictionary<int, ShoppingStoreModel>();
                foreach (var shoppingStore in shoppingStores)
                {
                    dictionary.Add(counter, shoppingStore);
                    counter++;
                }

                foreach (var shoppingStore in dictionary)
                {
                    Console.WriteLine($"{shoppingStore.Key}. {shoppingStore.Value.Name}");
                }

                Console.WriteLine("\nSelect an option. Enter 0 to go back to Main Menu");
                Console.WriteLine("-----------------------------------");

                int userInputShoppingStore;
                while (!int.TryParse(Console.ReadLine(), out userInputShoppingStore))
                    Console.WriteLine("You entered an invalid key, please enter a valid key");

                if (userInputShoppingStore == 0)
                {
                    await Program.ChooseShoppingStoreMainMenu(shoppingStores);
                }

                foreach (var shoppingStore in dictionary)
                {
                    if (userInputShoppingStore == shoppingStore.Key)
                    {
                        await ShoppingStoreManagementStartNavigate(shoppingStore.Value);
                    }
                }
            }
        }

        public static async Task ShoppingStoreManagementStartNavigate(ShoppingStoreModel shoppingStore)
        {
            bool isContinue = true;
            while (isContinue)
            {
                Console.Clear();
                Console.WriteLine("------------------------------");
                Console.WriteLine("Choose what you would like to do:");
                Console.WriteLine("1. Add a new product");
                Console.WriteLine("2. Delete a product");
                Console.WriteLine("3. Edit product");
                Console.WriteLine("------------------------------");
                Console.WriteLine("Go back with '0' ");

                string userInput = Console.ReadLine();

                switch (userInput)
                {
                    case "1":
                        isContinue = false;
                        await AddNewProduct(shoppingStore);
                        break;
                    case "2":
                        isContinue = false;
                        await DeleteProduct(shoppingStore);
                        break;
                    case "3":
                        isContinue = false;
                        await EditProduct(shoppingStore);
                        break;
                    case "0":
                        isContinue = false;
                        var shoppingStores = await ShoppingStore.GetAllShoppingStores();
                        await Program.ChooseShoppingStoreMainMenu(shoppingStores);
                        break;
                }
            }
        }

        public static async Task AddNewProduct(ShoppingStoreModel shoppingStoreModel)
        {
            bool isContinueAddNewProduct = true;
            while (isContinueAddNewProduct)
            {
                Console.Clear();
                Console.WriteLine("------------------------------");
                Console.WriteLine("You are adding a new product");
                Console.WriteLine("------------------------------");
                Console.WriteLine("Here is the product list of this shopping store");
                Console.WriteLine("------------------------------");

                var shoppingStore = await ShoppingStore.GetShoppingStore(shoppingStoreModel.Id);

                var counter = 1;
                foreach (var product in shoppingStore.Products)
                {
                    Console.WriteLine($"{counter}. {product.Name}");
                    counter++;
                }

                Console.WriteLine("-----------------------------------");
                Console.WriteLine("Please press any key to add a new product or 0 for going back");
                var userInput = Console.ReadLine();
                if (userInput != "0")
                {
                    Console.WriteLine("-----------------------------------");
                    Console.WriteLine("Please enter product name: ");
                    var productName = Console.ReadLine();
                    Console.Write("Please enter price: ");
                    decimal productPrice;
                    while (!decimal.TryParse(Console.ReadLine(), out productPrice))
                        Console.WriteLine("You entered an invalid key, please enter a valid key");

                    Console.Write("Please enter amount: ");
                    int productAmount;
                    while (!int.TryParse(Console.ReadLine(), out productAmount))
                        Console.WriteLine("You entered an invalid key, please enter a valid key");

                    var newProduct = new ProductModel(productName, productPrice, productAmount);

                    await ShoppingStore.InsertProductToProducts(newProduct);

                    var getProductFromDb = await Product.GetProductByName(newProduct.Name);
                    await ShoppingStore.InsertProductToShoppingStore(shoppingStoreModel, getProductFromDb);
                }
                else
                {
                    await ShoppingStoreManagementStartNavigate(shoppingStore);
                }
            }
        }

        public static async Task DeleteProduct(ShoppingStoreModel shoppingStore)
        {
            bool isContinueDeleteProduct = true;
            while (isContinueDeleteProduct)
            {
                Console.Clear();
                Console.WriteLine("------------------------------");
                Console.WriteLine("You are deleting products");
                Console.WriteLine("------------------------------");


                var counter = 1;
                var shoppingStoreFromDb = await ShoppingStore.GetShoppingStore(shoppingStore.Id);
                var dictionary = new Dictionary<int, ProductModel>();
                foreach (var product in shoppingStoreFromDb.Products)
                {
                    dictionary.Add(counter, product);
                    counter++;
                }

                foreach (var product in dictionary)
                {
                    Console.WriteLine($"{product.Key}. {product.Value.Name}");
                }

                Console.WriteLine("\nWhich product would you like to delete?");
                Console.WriteLine("------------------------------");
                Console.WriteLine("Go back with '0' ");
                var deleteProduct = Console.ReadLine();

                if (deleteProduct == "0")
                {
                    isContinueDeleteProduct = false;
                    await ShoppingStoreManagementStartNavigate(shoppingStore);
                }

                foreach (var item in dictionary)
                {
                    if (deleteProduct == item.Key.ToString())
                    {
                        Console.WriteLine($"Product to delete is: {item.Value.Name}");
                        await ShoppingStore.DeleteProductInShoppingStore(shoppingStore,item.Value);
                        Console.WriteLine($"Successfully deleted '{item.Value.Name}'. Press any key to continue deleting products.");
                        Console.ReadKey();
                    }
                }
            }
        }

        public static async Task EditProduct(ShoppingStoreModel shoppingStore)
        {
            bool isContinueEditProduct = true;
            while (isContinueEditProduct)
            {
                Console.Clear();
                Console.WriteLine("------------------------------");
                Console.WriteLine("You are editing products");
                Console.WriteLine("------------------------------");
                Console.WriteLine("Here is the product list of this shopping store");
                Console.WriteLine("------------------------------");

                var shoppingStoreFromDb = await ShoppingStore.GetShoppingStore(shoppingStore.Id);

                var counter = 1;
                var dictionary = new Dictionary<int, ProductModel>();
                foreach (var product in shoppingStoreFromDb.Products)
                {
                    dictionary.Add(counter, product);
                    counter++;
                }

                foreach (var product in dictionary)
                {
                    var stockBalanceText = product.Value.StockBalance <= 0
                        ? "out of stock"
                        : $"x {product.Value.StockBalance}";

                    Console.WriteLine($"{product.Key}. {product.Value.Name} x{product.Value.StockBalance}");
                }

                Console.WriteLine("-----------------------------------");
                Console.WriteLine("Please press any key to continue or 0 for going back");
                var userInput = Console.ReadLine();
                if (userInput == "0")
                {
                    await ShoppingStoreManagementStartNavigate(shoppingStore);
                }
                else
                {
                    Console.WriteLine("-----------------------------------");
                    Console.Write("Please pick the product you would like to edit the stock balance of the product:");
                    int editProduct = Convert.ToInt32(Console.ReadLine());

                    foreach (var product in dictionary)
                    {
                        if (editProduct == product.Key)
                        {
                            Console.WriteLine($"{product.Value.Name} picked.");
                            Console.WriteLine("How many of this product would you like to have?");
                            int editAmountOfProduct = Convert.ToInt32(Console.ReadLine());
                            await ShoppingStore.UpdateShoppingStoreProductStockBalance(shoppingStore, product.Value,editAmountOfProduct);

                            Console.WriteLine("Thank you, we will change that. Press any key for going back");
                            Console.ReadKey();
                            break;
                        }
                    }

                    // await db.ProductCollection.save(editProduct);
                    
                }
               

            }

        }
    }
}
