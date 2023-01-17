using ShoppingStoreApp.DataAccess;
using ShoppingStoreApp.DataAccess.Models;

namespace ShoppingStoreApp
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            await Product.CreateInitialProducts();
            await ShoppingStore.CreateInitialShoppingStores();
            await Customer.CreateInitialCustomers();

            ShoppingStoreDataAccess db = new ShoppingStoreDataAccess();
            var shoppingStore = await db.GetAllShoppingStore();

            await ChooseShoppingStoreMainMenu(shoppingStore);
        }

        public static async Task ChooseShoppingStoreMainMenu(List<ShoppingStoreModel> shoppingStores)
        {
            bool isContinueChooseShoppingStoreMainMenu = true;

            while (isContinueChooseShoppingStoreMainMenu)
            {
                Console.Clear();
                Console.WriteLine("-----------------------------------");
                Console.WriteLine("*** Welcome! Which shopping store would you like to enter? ***");
                Console.WriteLine("-----------------------------------");

                var counter = 1;
                var dictionary = new Dictionary<int, ShoppingStoreModel>();
                foreach (var shopppingStore in shoppingStores)
                {
                    dictionary.Add(counter, shopppingStore);
                    counter++;
                }

                foreach (var shoppingStore in dictionary)
                {
                    Console.WriteLine($"{shoppingStore.Key}. {shoppingStore.Value.Name}");
                }

                Console.WriteLine("\nSelect an option");
                Console.WriteLine("-----------------------------------");

                int userInputShoppingStore;
                while (!int.TryParse(Console.ReadLine(), out userInputShoppingStore))
                    Console.WriteLine("You entered an invalid key, please enter a valid key");

                foreach (var shoppingStore in dictionary)
                {
                    if (userInputShoppingStore == shoppingStore.Key)
                    {
                        await Customer.SignIn(shoppingStore.Value);
                    }
                }
            }
        }
    }
}