using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServiceLayer.Services;
using DataAccessLayer.Models;
using DataAccessLayer.Database;


namespace UnitTesting
{
   
    [TestClass]
    public class ClientServiceUT
    {
        Client client;
        ClientService cs;
        DatabaseContext _db;
        TestingUtils tu;

        public ClientServiceUT()
        {
            cs = new ClientService();
            tu = new TestingUtils();
            
        }

        [TestMethod]
        public void CreateClient()
        {
            //Arrange
            Client newClient = tu.CreateClientObject();
            var expectedResponse = newClient;

            using (_db = tu.CreateDataBaseContext()) {
                //Act
                Client response = cs.CreateClient(_db, newClient);
                _db.SaveChanges();

                //Assert
                var result = _db.Clients.Find(newClient.Id);
                Assert.IsNotNull(response);
                Assert.IsNotNull(result);
                Assert.AreSame(result, expectedResponse);

            }
        }

        [TestMethod]
        public void DeleteClient()
        {
            //Arrange
            Client newClient = tu.CreateClientInDb();
            var expectedResponse = newClient;

            using (_db = tu.CreateDataBaseContext())
            {
                //Act
                var response = cs.DeleteClient(_db, newClient.Id);
                _db.SaveChanges();
                var result = _db.Clients.Find(newClient.Id);
                //Assert

                Assert.IsNotNull(response);
                Assert.IsNull(result);
                Assert.AreEqual(response.Id, expectedResponse.Id);

            }
        }
        [TestMethod]
        public void GetClient()
        {
            // Arrange 

            Client newUser = tu.CreateClientInDb();
            var expectedResult = newUser;

            // ACT
            using (_db = tu.CreateDataBaseContext())
            {
                var result = cs.GetClient(_db, expectedResult.Id);

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(expectedResult.Id, result.Id);
            }
        }

        [TestMethod]
        public void UpdateClient() {
            // Arrange
            Client newClient = tu.CreateClientInDb();
            newClient.Address = "123 What Ave.";
            var expectedResponse = newClient;
            var expectedResult = newClient;

            // ACT
            using (_db = tu.CreateDataBaseContext())
            {
                var response = cs.UpdateClient(_db, newClient);
                _db.SaveChanges();
                var result = _db.Clients.Find(expectedResult.Id);

                // Assert
                Assert.IsNotNull(response);
                Assert.IsNotNull(result);
                Assert.AreEqual(result.Id, expectedResult.Id);
                Assert.AreEqual(result.Address, expectedResult.Address);
            }

        }


    }
        
    
}
