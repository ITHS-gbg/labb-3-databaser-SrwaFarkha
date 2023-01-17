using MongoDB.Driver;
using ShoppingStoreApp.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingStoreApp.DataAccess
{
    public class ShoppingStoreDataAccess
    {
        private const string ConnectionString = "mongodb://localhost:27017";
        private const string DatabaseName = "SrwasShoppingStoreDb";
        private const string CustomerCollection = "customers";
        private const string OrderDetailCollection = "orderDetails";
        private const string ProductCollection = "products";
        private const string ShoppingStoreCollection = "shoppingStores";

        private IMongoCollection<T> ConnectToMongo<T>(in string collection)
        {
            var client = new MongoClient(ConnectionString);
            var db = client.GetDatabase(DatabaseName);

            return db.GetCollection<T>(collection);
        }

        public Task CreateCustomer(CustomerModel customer)
        {
            var customerCollection = ConnectToMongo<CustomerModel>(CustomerCollection);
            return customerCollection.InsertOneAsync(customer);
        }

        public Task CreateOrderDetails(OrderDetailsModel orderDetail)
        {
            var orderDetailCollection = ConnectToMongo<OrderDetailsModel>(OrderDetailCollection);
            return orderDetailCollection.InsertOneAsync(orderDetail);
        }

        public Task CreateProduct(ProductModel product)
        {
            var productCollection = ConnectToMongo<ProductModel>(ProductCollection);
            return productCollection.InsertOneAsync(product);
        }

        public Task CreateShoppingStore(ShoppingStoreModel shoppingStore)
        {
            var shoppingStoreCollection = ConnectToMongo<ShoppingStoreModel>(ShoppingStoreCollection);
            return shoppingStoreCollection.InsertOneAsync(shoppingStore);
        }

        public async Task<CustomerModel> GetCustomerByName(string username)
        {
            var customerCollection = ConnectToMongo<CustomerModel>(CustomerCollection);
            var filter = Builders<CustomerModel>.Filter.Eq(x => x.Username, username);
            var result = await customerCollection.Find(filter).FirstOrDefaultAsync();
            return result;
        }

        public async Task<List<CustomerModel>> GetAllCustomer()
        {
            var customerCollection = ConnectToMongo<CustomerModel>(CustomerCollection);
            var result = await customerCollection.FindAsync(_ => true);
            return result.ToList();
        }

        public async Task<List<ShoppingStoreModel>> GetAllShoppingStore()
        {
            var shoppingStoreCollection = ConnectToMongo<ShoppingStoreModel>(ShoppingStoreCollection);
            var result = await shoppingStoreCollection.FindAsync(_ => true);
            return result.ToList();
        }

        public async Task<List<ProductModel>> GetAllProducts()
        {
            var productCollection = ConnectToMongo<ProductModel>(ProductCollection);
            var result = await productCollection.FindAsync(_ => true);
            return result.ToList();
        }

        public Task UpdateCustomer(CustomerModel newCustomer)
        {
            var customerCollection = ConnectToMongo<CustomerModel>(CustomerCollection);
            var oldCustomer = Builders<CustomerModel>.Filter.Eq(x => x.Id, newCustomer.Id);
            return customerCollection.ReplaceOneAsync(oldCustomer, newCustomer, new ReplaceOptions { IsUpsert = true });
        }

        public Task DeleteCustomer(CustomerModel customer)
        {
            var customerCollection = ConnectToMongo<CustomerModel>(CustomerCollection);
            return customerCollection.DeleteOneAsync(x => x.Id == customer.Id);
        }

        public async Task DeleteProductFromStoreStockBalance(ShoppingStoreModel shoppingStore, ProductModel product)
        {
            var shoppingStoreCollection = ConnectToMongo<ShoppingStoreModel>(ShoppingStoreCollection);
            //WIP REMOVE PRODUCT

            //return shoppingStoreCollection.ReplaceOneAsync(oldProduct, newProduct);
            //return shoppingStoreCollection.DeleteOneAsync(x => x.Id == customer.Id);
        }
    }
}

