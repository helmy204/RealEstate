using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson.IO;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    [TestClass]
    public class PocoTests
    {
        public PocoTests()
        {
            JsonWriterSettings.Defaults.Indent = true;
        }

        public class Person
        {
            public string FirstName { get; set; }
            public int Age { get; set; }

            public List<string> Address = new List<string>();
            public Contact Contact = new Contact();
        }

        public class Contact
        {
            public string Email { get; set; }
            public string Phone { get; set; }
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
