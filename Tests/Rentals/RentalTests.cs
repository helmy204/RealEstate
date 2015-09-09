using Microsoft.VisualStudio.TestTools.UnitTesting;
using RealEstate.Rentals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace Tests.Rentals
{
    [TestClass]
    public class RentalTests
    {
        // Trible A Syntax
        // A : Action you are testing
        // A : Arrangement or the scenario of the test
        // A : Assertion
        [TestMethod]
        public void ToDocument_RentalWithPrice_PriceRepresentedAsDouble()
        {
            // Arrange
            var rental = new Rental();
            rental.Price = 1;

            // Act
            var document = rental.ToBsonDocument();
            
            // Assert
            Assert.AreEqual(document["Price"].BsonType, BsonType.Double);
        }

        [TestMethod]
        public void ToDocument_RentalWithAnId_IdIsRepresentedAsAnObjectId()
        {
            // Arrange
            var rental = new Rental();
            rental.Id = ObjectId.GenerateNewId().ToString();

            // Act
            var document = rental.ToBsonDocument();

            // Assert
            Assert.AreEqual(document["_id"].BsonType, BsonType.ObjectId);

        }
    }
}
