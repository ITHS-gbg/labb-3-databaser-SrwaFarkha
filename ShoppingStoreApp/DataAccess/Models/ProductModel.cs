using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingStoreApp.DataAccess.Models
{
    public class ProductModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }

        public int StockBalance { get; set; }

        public ProductModel(string name, decimal price, int stockBalance = 10)
        {
            Name = name;
            Price = price;
            StockBalance = stockBalance;
        }

        public decimal GetPrice(CustomerModel customer)
        {
            switch (customer.CustomerLevelType)
            {
                case Enums.CustomerLevelType.Bronze:
                    return Price - (Price * 5 / 100);
                case Enums.CustomerLevelType.Silver:
                    return Price - (Price * 10 / 100);
                case Enums.CustomerLevelType.Gold:
                    return Price - (Price * 15 / 100);
                default:
                    return Price;
            }
        }
    }

}
