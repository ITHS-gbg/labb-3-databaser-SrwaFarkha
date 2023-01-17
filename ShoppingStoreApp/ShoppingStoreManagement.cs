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
                        DeleteProduct();
                        break;
                    case "3":
                        isContinue = false;
                        EditProduct();
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

        public static void DeleteProduct()
        {
            //bool isContinueDeleteBook = true;
            //while (isContinueDeleteBook)
            //{
            //    Console.Clear();
            //    Console.WriteLine("------------------------------");

            //    var counter = 1;
            //    var books = _böckerRepository.GetAll();
            //    var dictionary = new Dictionary<int, Böcker>();
            //    foreach (var book in books)
            //    {
            //        dictionary.Add(counter, book);
            //        counter++;
            //    }

            //    foreach (var book in dictionary)
            //    {
            //        Console.WriteLine($"{book.Key}. {book.Value.Titel}");
            //    }

            //    Console.WriteLine("\nWhich book would you like to delete?");
            //    Console.WriteLine("------------------------------");
            //    Console.WriteLine("Go back with 'b' ");
            //    var deleteBook = Console.ReadLine();

            //    if (deleteBook == "b")
            //    {
            //        isContinueDeleteBook = false;
            //        ShoppingStoreManagementStartNavigate();
            //    }

            //    foreach (var item in dictionary)
            //    {
            //        if (deleteBook == item.Key.ToString())
            //        {
            //            isContinueDeleteBook = false;
            //            _böckerRepository.DeleteBook(item.Value.Isbn13);
            //            Console.WriteLine($"Successfully deleted '{item.Value.Titel}'. Press any key to go back.");
            //            Console.ReadKey();
            //            ShoppingStoreManagementStartNavigate();
            //        }
            //    }
            //}
        }

        public static void EditProduct()
        {

        }
    }
}
