using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingStoreApp.DataAccess.Models
{
    public class ShoppingStoreModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        public List<ProductModel> Products { get; set; }

        public ShoppingStoreModel(string name, List<ProductModel> products)
        {
            Name = name;
            Products = products;
        }
    }
}
