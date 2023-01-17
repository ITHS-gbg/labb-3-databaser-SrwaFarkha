using ShoppingStoreApp.DataAccess.Models;
using ShoppingStoreApp.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingStoreApp
{
    public class ShoppingStore
    {

        public static async Task CreateInitialShoppingStores()
        {
            var shoppingStores = await GetAllShoppingStores();
            if (shoppingStores.Count > 0)
            {
                return;
            }

            ShoppingStoreDataAccess db = new ShoppingStoreDataAccess();

            var products = await db.GetAllProducts();
            var random = new Random();

            var srwasProducts = products
                .OrderBy(x => random.Next())
                .Take(6)
                .ToList();

            var niklasProducts = products
                .OrderBy(x => random.Next())
                .Take(6)
                .ToList();


            var newShoppingStores = new List<ShoppingStoreModel>
            {
                new ShoppingStoreModel("Srwas shoppingstore", srwasProducts),
                new ShoppingStoreModel("Niklas shoppingstore", niklasProducts)
            };

            foreach (var item in newShoppingStores)
            {
                await db.CreateShoppingStore(item);
            }
        }

        public static async Task<List<ShoppingStoreModel>> GetAllShoppingStores()
        {
            ShoppingStoreDataAccess db = new ShoppingStoreDataAccess();
            var result = await db.GetAllShoppingStore();
            return result;
        }
    }
}

