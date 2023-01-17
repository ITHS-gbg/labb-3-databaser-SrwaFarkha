using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingStoreApp.DataAccess.Models
{
    public class OrderDetailsModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string CustomerName { get; set; }
        public DateTime OrderDate { get; set; }
        public string DeliveryAddress { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public string ShoppingStoreName { get; set; }
        public List<ProductModel> Products { get; set; }
    }
}

