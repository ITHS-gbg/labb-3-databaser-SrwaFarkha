using ShoppingStoreApp.DataAccess.Models;
using ShoppingStoreApp.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingStoreApp
{
    public class Product
    {
        public static async Task CreateInitialProducts()
        {
            var products = await GetAllProducts();
            if (products.Count > 0)
            {
                return;
            }

            var newProducts = new List<ProductModel>
            {
                new ProductModel("Apple", 5.0M),
                new ProductModel("Banana", 7.0M),
                new ProductModel("Pear", 6.0M),
                new ProductModel("Strawberry", 1.0M),
                new ProductModel("Melon", 10.0M),
                new ProductModel("Kiwi", 4.0M),
                new ProductModel("Coconut", 15.0M),
                new ProductModel("Pineapple", 18.0M),
                new ProductModel("Blueberries", 5.0M),
                new ProductModel("Papaya", 5.0M),
                new ProductModel("Passion fruit", 10.0M),
            };

            ShoppingStoreDataAccess db = new ShoppingStoreDataAccess();
            foreach (var item in newProducts)
            {
                await db.CreateProduct(item);
            }
        }

        private static async Task<List<ProductModel>> GetAllProducts()
        {
            ShoppingStoreDataAccess db = new ShoppingStoreDataAccess();
            var result = await db.GetAllProducts();
            return result;
        }
    }
}
