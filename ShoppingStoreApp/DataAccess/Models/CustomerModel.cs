using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingStoreApp.DataAccess.Models
{
    public class CustomerModel
    {

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }

        public Enums.CustomerLevelType CustomerLevelType { get; set; }
        public Enums.CurrencyType CurrencyType { get; set; }
        public List<ProductModel>? ShoppingCart { get; set; }


        public CustomerModel(string userName, string password, string phoneNumber, string address, string city, string postalCode, Enums.CustomerLevelType customerLevelType, Enums.CurrencyType currencyType, List<ProductModel>? shoppingCart)
        {
            Username = userName;
            Password = password;
            PhoneNumber = phoneNumber;
            Address = address;
            City = city;
            PostalCode = postalCode;
            CustomerLevelType = customerLevelType;
            CurrencyType = currencyType;
            ShoppingCart = shoppingCart;
        }

    }

}
