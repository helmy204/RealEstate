using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson.IO;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization;

namespace Tests
{
    [TestClass]
    public class PocoTests
    {
        public PocoTests()
        {
            JsonWriterSettings.Defaults.Indent = true;
        }

        [BsonIgnoreExtraElements]
        public class Person
        {
            [BsonId]
            public int PersonId { get; set; }

            public string FirstName { get; set; }
            public int Age { get; set; }

            public List<string> Address = new List<string>();
            
            public Contact Contact = new Contact();
            
            [BsonIgnore]
            public string IgnoreMe { get; set; }
            [BsonElement("New")]
            public string Old { get; set; }
            private string Encapsulated;
            [BsonElement]
            private string Encapsulated2;
            [BsonIgnoreIfNull]
            public string BsonIgnoreIfNullElement { get; set; }
            
            [BsonRepresentation(BsonType.Double)]
            public decimal NetWorth { get; set; }

            public DateTime BirthTime { get; set; }
            [BsonDateTimeOptions(Kind=DateTimeKind.Local)]
            public DateTime BirthTimeLocal { get; set; }

            public DateTime BirthDate { get; set; }
            [BsonDateTimeOptions(DateOnly=true)]
            public DateTime BirthDateOnly { get; set; }
        }

        public class Contact
        {
            public string Email { get; set; }
            public string Phone { get; set; }
        }

        [TestMethod]
        public void SerializationAttributes()
        {
            var person = new Person();
            person.NetWorth = 100.5m;
            person.BirthTime = new DateTime(2014, 1, 2, 11, 30, 0);
            person.BirthTimeLocal = new DateTime(2014, 1, 2, 11, 30, 0);

            person.BirthDate = new DateTime(2014, 1, 2);
            person.BirthDateOnly = new DateTime(2014, 1, 2);

            var personJson = person.ToJson();
            Console.WriteLine(personJson);

            var personObject = BsonSerializer.Deserialize<Person>(personJson);
            Console.WriteLine(personObject.BirthTime);
            Console.WriteLine(personObject.BirthTimeLocal);
        }

        [TestMethod]
        public void Automatic()
        {
            var person = new Person
            {
                Age = 54,
                FirstName = "bob"
            };

            person.Address.Add("101 Some Road");
            person.Address.Add("Unit 501");

            person.Contact.Email = "email@email.com";
            person.Contact.Phone = "123-456-7890";

            Console.WriteLine(person.ToJson());
        }
    }
}
