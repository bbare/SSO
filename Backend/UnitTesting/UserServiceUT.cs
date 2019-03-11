using System;
using System.Data.Entity.Validation;
using DataAccessLayer.Database;
using DataAccessLayer.Models;
using ManagerLayer.UserManagement;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServiceLayer.Services;

namespace UnitTesting
{
    [TestClass]
    public class UserServiceUT
    {
        User newUser;
        TestingUtils tu;
        UserService us;
        DatabaseContext _db;
        UserManagementManager _umm;

        public UserServiceUT()
        {
            us = new UserService();
            tu = new TestingUtils();
            _umm = new UserManagementManager();
        }

        [TestMethod]
        public void Create_User_Success()
        {
            // Arrange
            newUser = tu.CreateUserObject();
            var expected = newUser;
            using (_db = tu.CreateDataBaseContext())
            {
                // Act
                var response = us.CreateUser(_db, newUser);
                _db.SaveChanges();

                //Assert
                Assert.IsNotNull(response);
                Assert.AreSame(response, expected);
            }
        }

        [TestMethod]
        public void Create_User_RetrieveNew_Success()
        {
            // Arrange
            newUser = tu.CreateUserObject();
            var expected = newUser;

            using (_db = tu.CreateDataBaseContext())
            {
                // Act
                User response = us.CreateUser(_db, newUser);
                _db.SaveChanges();

                //Assert
                var result = _db.Users.Find(newUser.Id);
                Assert.IsNotNull(response);
                Assert.IsNotNull(result);
                Assert.AreSame(result, expected);
            }
        }

        [TestMethod]
        public void Create_User_Fail_ExceptionThrown()
        {
            // Arrange
            newUser = new User
            {
                Email = Guid.NewGuid() + "@" + Guid.NewGuid() + ".com",
                DateOfBirth = DateTime.UtcNow,
                City = "Los Angeles",
                State = "California",
                Country = "United States",
                
                // missing required fields
            };
            var expected = newUser;

            using (_db = tu.CreateDataBaseContext())
            {
                // ACT
                var response = us.CreateUser(_db, newUser);
                try
                {
                    _db.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    //catch error
                    // detach user attempted to be created from the db context - rollback
                    _db.Entry(response).State = System.Data.Entity.EntityState.Detached;
                }
                var result = _db.Users.Find(newUser.Id);

                // Assert
                Assert.IsNull(result);
                Assert.IsNotNull(response);
                Assert.AreEqual(expected, response);
                Assert.AreNotEqual(expected, result);
            }
        }

        [TestMethod]
        public void Create_User_Using_Manager()
        {
            // Arrange
            string email = Guid.NewGuid() + "@" + Guid.NewGuid() + ".com";
            string password = (Guid.NewGuid()).ToString();
            DateTime dob = DateTime.UtcNow;

            // Act
            var response =_umm.CreateUser(email, password, dob);
            var result = _umm.GetUser(response.Id);

            // Assert 
            Assert.IsNotNull(response);
            Assert.IsNotNull(result);
            Assert.AreEqual(email, result.Email);
        }

        [TestMethod]
        public void Create_User_Using_Manager_NotRealEmail()
        {
            // Arrange
            string email = Guid.NewGuid() + ".com";
            string password = (Guid.NewGuid()).ToString();
            DateTime dob = DateTime.UtcNow;

            // Act
            var response = _umm.CreateUser(email, password, dob);

            // Assert 
            Assert.IsNull(response);
        }

        [TestMethod]
        public void Delete_User_Success()
        {
            // Arrange
            newUser = tu.CreateUserInDb();

            var expectedResponse = newUser;

            using (_db = tu.CreateDataBaseContext())
            {
                // Act
                var response = us.DeleteUser(_db, newUser.Id);
                _db.SaveChanges();
                var result = _db.Users.Find(expectedResponse.Id);

                // Assert
                Assert.IsNotNull(response);
                Assert.IsNull(result);
                Assert.AreEqual(response.Id, expectedResponse.Id);
            }
        }

        [TestMethod]
        public void Delete_User_NonExisting()
        {
            // Arrange
            Guid nonExistingId = Guid.NewGuid();

            var expectedResponse = nonExistingId;

            using (_db = new DatabaseContext())
            {
                // Act
                var response = us.DeleteUser(_db, nonExistingId);
                // will return null if user does not exist
                _db.SaveChanges();
                var result = _db.Users.Find(expectedResponse);

                // Assert
                Assert.IsNull(response);
                Assert.IsNull(result);
            }
        }

        [TestMethod]
        public void Update_User_Success()
        {
            // Arrange
            newUser = tu.CreateUserInDb();
            newUser.City = "Long Beach";
            var expectedResponse = newUser;
            var expectedResult = newUser;

            // ACT
            using (_db = tu.CreateDataBaseContext())
            {
                var response = us.UpdateUser(_db, newUser);
                _db.SaveChanges();
                var result = _db.Users.Find(expectedResult.Id);

                // Assert
                Assert.IsNotNull(response);
                Assert.IsNotNull(result);
                Assert.AreEqual(result.Id, expectedResult.Id);
                Assert.AreEqual(result.City, expectedResult.City);
            }

        }

        [TestMethod]
        public void Update_User_NonExisting_why()
        {
            // Arrange
            newUser = tu.CreateUserObject();
            newUser.City = "Long Beach";
            var expectedResponse = newUser;
            var expectedResult = newUser;

            // ACT
            using (_db = tu.CreateDataBaseContext())
            {
                var response = us.UpdateUser(_db, newUser);
                try
                {
                    _db.SaveChanges();
                }
                catch (Exception)
                {
                    // catch error
                    // rollback changes
                    _db.Entry(newUser).State = System.Data.Entity.EntityState.Detached;
                }
                var result = _db.Users.Find(expectedResult.Id);

                // Assert
                Assert.IsNotNull(response);
                Assert.IsNull(result);
            }
        }

        [TestMethod]
        public void Update_User_OnRequiredValue()
        {
            // Arrange
            newUser = tu.CreateUserInDb();
            var expectedResult = newUser;
            newUser.PasswordHash = null;
            var expectedResponse = newUser;

            // ACT
            using (_db = tu.CreateDataBaseContext())
            {
                var response = us.UpdateUser(_db, newUser);
                try
                {
                    _db.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    // catch error
                    // rollback changes
                    _db.Entry(response).CurrentValues.SetValues(_db.Entry(response).OriginalValues);
                    _db.Entry(response).State = System.Data.Entity.EntityState.Unchanged;
                }
                var result = _db.Users.Find(expectedResult.Id);

                // Assert
                Assert.IsNotNull(response);
                Assert.AreEqual(expectedResponse, response);
                Assert.IsNotNull(result);
                Assert.AreEqual(expectedResult, result);
            }
        }

        [TestMethod]
        public void Get_User_Success()
        {
            // Arrange 

            newUser = tu.CreateUserInDb();
            var expectedResult = newUser;

            // ACT
            using (_db = tu.CreateDataBaseContext())
            {
                var result = us.GetUser(_db, expectedResult.Id);

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(expectedResult.Id, result.Id);
            }
        }

        [TestMethod]
        public void Get_User_NonExisting()
        {
            // Arrange
            Guid nonExistingUser = Guid.NewGuid();
            User expectedResult = null;

            // Act
            using (_db = tu.CreateDataBaseContext())
            {
                var result = us.GetUser(_db, nonExistingUser);

                // Assert
                Assert.IsNull(result);
                Assert.AreEqual(expectedResult, result);
            }
        }

        [TestMethod]
        public void Disable_User_Success()
        {
            // Arrange
            newUser = tu.CreateUserInDb();
            var expectedResponse = newUser;
            var expectedResult = true;

            // ACT
            var response = _umm.DisableUser(newUser);
            var result = _umm.GetUser(newUser.Id);

            // Assert
            Assert.IsTrue(response == 1);
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult, result.Disabled);
        }

        [TestMethod]
        public void Enable_User_Success()
        {
            // Arrange
            newUser = tu.CreateUserInDb();
            var expectedResponse = newUser;
            var expectedResult = false;

            // ACT
            var response = _umm.EnableUser(newUser);
            var result = _umm.GetUser(newUser.Id);

            // Assert
            Assert.IsTrue(response == 1);
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult, result.Disabled);
        }

        [TestMethod]
        public void Toggle_User_Success()
        {
            // Arrange
            User newUser;
            using (var _db = tu.CreateDataBaseContext())
            {
                newUser = tu.CreateUserObject();
                _db.Users.Add(newUser);
                _db.SaveChanges();
            }
            var expectedResponse = newUser;
            var expectedResult = newUser.Disabled;

            // ACT
            var response = _umm.ToggleUser(newUser, null);

            var result = _umm.GetUser(newUser.Id);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response == 1);
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResponse.Id, result.Id);
        }
    }
}
