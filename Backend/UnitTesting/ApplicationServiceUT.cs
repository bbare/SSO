using System;
using ServiceLayer.Services;
using DataAccessLayer.Database;
using DataAccessLayer.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.Entity.Validation;

namespace UnitTesting
{
    /// <summary>
    /// Tests Application Services
    /// </summary>
    [TestClass]
    public class ApplicationServiceUT
    {
        DatabaseContext _db;
        ApplicationService aps;
        TestingUtils tu;
        Application newApp;

        public ApplicationServiceUT()
        {
            _db = new DatabaseContext();
            tu = new TestingUtils();
            aps = new ApplicationService();
        }


        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void CreateApplication_Pass_ReturnApp()
        {
            // Arrange
            newApp = tu.CreateApplicationObject();
            var expected = newApp;

            using (_db = tu.CreateDataBaseContext())
            {
                // Act
                var response = aps.CreateApplication(_db, newApp);
                _db.SaveChanges();

                // Assert
                Assert.IsNotNull(response);
                Assert.AreEqual(response.Id, expected.Id);

                aps.DeleteApplication(_db, response.Id);
                _db.SaveChanges();
            }
        }

        [TestMethod]
        public void CreateApplication_Fail_ExistingAppShouldReturnNull()
        {
            // Arrange
            newApp = tu.CreateApplicationObject();
            var expected = newApp;

            using (_db = tu.CreateDataBaseContext())
            {
                // Act
                var response = aps.CreateApplication(_db, newApp);
                _db.SaveChanges();

                var actual = aps.CreateApplication(_db, newApp);

                // Assert
                Assert.IsNull(actual);
                Assert.AreNotEqual(expected, actual);

                aps.DeleteApplication(_db, response.Id);
                _db.SaveChanges();
            }
        }

        [TestMethod]
        public void CreateApplication_Fail_MissingFieldsShouldThrowException()
        {
            // Arrange
            newApp = tu.CreateApplicationObject();
            newApp.Title = null;
            var expected = newApp;

            using (_db = tu.CreateDataBaseContext())
            {
                // Act
                var response = aps.CreateApplication(_db, newApp);
                try
                {
                    _db.SaveChanges();
                }
                catch (DbEntityValidationException)
                {
                    // Catch error
                    // Detach Session attempted to be created from the db context - rollback
                    _db.Entry(response).State = System.Data.Entity.EntityState.Detached;
                }
                var result = _db.Applications.Find(newApp.Id);

                // Assert
                Assert.IsNull(result);
                Assert.IsNotNull(response);
                Assert.AreEqual(expected, response);
                Assert.AreNotEqual(expected, result);
            }
        }

        [TestMethod]
        public void CreateApplication_Fail_NullValuesReturnNull()
        {
            using (_db = tu.CreateDataBaseContext())
            {
                // Act
                var result = aps.CreateApplication(_db, null);

                // Assert
                Assert.IsNull(result);
            }
        }

        [TestMethod]
        public void DeleteApplication_Pass_ReturnApp()
        {
            // Arrange
            newApp = tu.CreateApplicationObject();

            using (_db = tu.CreateDataBaseContext())
            {
                // Act
                newApp = aps.CreateApplication(_db, newApp);
                var expected = newApp;

                _db.SaveChanges();

                var response = aps.DeleteApplication(_db, newApp.Id);
                _db.SaveChanges();
                var result = _db.Applications.Find(expected.Id);

                // Assert
                Assert.IsNotNull(response);
                Assert.IsNull(result);
                Assert.AreEqual(response.Id, expected.Id);
            }
        }

        [TestMethod]
        public void DeleteApplication_Fail_NonExistingAppShouldReturnNull()
        {
            // Arrange
            Guid nonExistingId = Guid.NewGuid();

            var expected = nonExistingId;

            using (_db = new DatabaseContext())
            {
                // Act
                var response = aps.DeleteApplication(_db, nonExistingId);
                _db.SaveChanges();
                var result = _db.Applications.Find(expected);

                // Assert
                Assert.IsNull(response);
                Assert.IsNull(result);
            }
        }

        [TestMethod]
        public void UpdateApplication_Pass_ReturnApp()
        {
            // Arrange
            newApp = tu.CreateApplicationObject();
            var expected = newApp.Title;

            // Act
            using (_db = tu.CreateDataBaseContext())
            {
                newApp = aps.CreateApplication(_db, newApp);
                _db.SaveChanges();

                newApp.Title = "A new title";
                var response = aps.UpdateApplication(_db, newApp);
                _db.SaveChanges();

                var result = _db.Applications.Find(newApp.Id);

                // Assert
                Assert.IsNotNull(response);
                Assert.IsNotNull(result);
                Assert.AreEqual(result.Id, newApp.Id);
                Assert.AreNotEqual(expected, result.Title);

                aps.DeleteApplication(_db, newApp.Id);
                _db.SaveChanges();
            }
        }

        [TestMethod]
        public void UpdateApplication_Fail_NonExistingAppShouldThrowException()
        {
            // Arrange
            newApp = tu.CreateApplicationObject();
            var expected = newApp;

            // Act
            using (_db = tu.CreateDataBaseContext())
            {
                var response = aps.UpdateApplication(_db, newApp);
                try
                {
                    _db.SaveChanges();
                }
                catch (System.Data.Entity.Infrastructure.DbUpdateConcurrencyException)
                {
                    _db.Entry(newApp).State = System.Data.Entity.EntityState.Detached;
                }
                catch (System.Data.Entity.Core.EntityCommandExecutionException)
                {
                    _db.Entry(newApp).State = System.Data.Entity.EntityState.Detached;
                }
                var result = _db.Applications.Find(expected.Id);

                // Assert
                Assert.IsNull(response);
                Assert.IsNull(result);
            }
        }

        [TestMethod]
        public void UpdateApplication_Fail_NullValuesReturnNull()
        {
            using (_db = tu.CreateDataBaseContext())
            {
                // Act
                var result = aps.UpdateApplication(_db, null);

                // Assert
                Assert.IsNull(result);
            }
        }

        [TestMethod]
        public void GetApplicationById_Pass_ReturnApp()
        {
            // Arrange
            newApp = tu.CreateApplicationObject();
            var expected = newApp;

            // Act
            using (_db = tu.CreateDataBaseContext())
            {
                newApp = aps.CreateApplication(_db, newApp);
                _db.SaveChanges();
                var result = aps.GetApplication(_db, newApp.Id);

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(expected.Id, result.Id);

                aps.DeleteApplication(_db, newApp.Id);
                _db.SaveChanges();
            }
        }

        [TestMethod]
        public void GetApplicationById_Fail_NonExistingAppShouldReturnNull()
        {
            // Arrange
            Guid nonExistingApp = Guid.NewGuid();
            Application expected = null;

            // Act
            using (_db = tu.CreateDataBaseContext())
            {
                var result = aps.GetApplication(_db, nonExistingApp);

                // Assert
                Assert.IsNull(result);
                Assert.AreEqual(expected, result);
            }
        }

        [TestMethod]
        public void GetApplicationByTitleEmail_Pass_ReturnApp()
        {
            // Arrange
            newApp = tu.CreateApplicationObject();
            var expected = newApp;

            // Act
            using (_db = tu.CreateDataBaseContext())
            {
                newApp = aps.CreateApplication(_db, newApp);
                _db.SaveChanges();
                var result = aps.GetApplication(_db, newApp.Title, newApp.Email);

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(expected.Title, result.Title);
                Assert.AreEqual(expected.Email, result.Email);

                aps.DeleteApplication(_db, newApp.Id);
                _db.SaveChanges();
            }
        }

        [TestMethod]
        public void GetApplicationByTitleEmail_Fail_NonExistingAppShouldReturnNull()
        {
            // Arrange
            string nonExistingTitle = "title";
            string nonExistingEmail = "email";
            Application expected = null;

            // Act
            using (_db = tu.CreateDataBaseContext())
            {
                var result = aps.GetApplication(_db, nonExistingTitle, nonExistingEmail);

                // Assert
                Assert.IsNull(result);
                Assert.AreEqual(expected, result);
            }
        }

        [TestMethod]
        public void GetApplicationByTitleEmail_Fail_NullValuesReturnNull()
        {
            using (_db = tu.CreateDataBaseContext())
            {
                // Act
                var result = aps.GetApplication(_db, null, null);

                // Assert
                Assert.IsNull(result);
            }
        }
    }
}
