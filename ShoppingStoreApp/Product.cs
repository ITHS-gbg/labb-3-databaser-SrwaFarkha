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
                new ProductModel("Apple", 5.0M, 10),
                new ProductModel("Banana", 7.0M, 10),
                new ProductModel("Pear", 6.0M, 10),
                new ProductModel("Strawberry", 1.0M, 10),
                new ProductModel("Melon", 10.0M, 10),
                new ProductModel("Kiwi", 4.0M, 10),
                new ProductModel("Coconut", 15.0M, 10),
                new ProductModel("Pineapple", 18.0M, 10),
                new ProductModel("Blueberries", 5.0M, 10),
                new ProductModel("Papaya", 5.0M, 10),
                new ProductModel("Passion fruit", 10.0M, 10),
            };

            ShoppingStoreDataAccess db = new ShoppingStoreDataAccess();
            foreach (var item in newProducts)
            {
                await db.CreateProduct(item);
            }
        }

        public static async Task<List<ProductModel>> GetAllProducts()
        {
            ShoppingStoreDataAccess db = new ShoppingStoreDataAccess();
            var result = await db.GetAllProducts();
            return result;
        }

        public static async Task<ProductModel> GetProductByName(string newProductName)
        {
            ShoppingStoreDataAccess db = new ShoppingStoreDataAccess();
            var result = await db.GetProductByName(newProductName);
            return result;
        }
    }
}
