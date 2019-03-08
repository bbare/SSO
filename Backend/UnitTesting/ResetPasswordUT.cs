using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServiceLayer.Services;
using DataAccessLayer.Database;
using DataAccessLayer.Models;
using System;

namespace UnitTesting
{
    [TestClass]
    class ResetPasswordUT
    {
        DatabaseContext _db;
        ResetService rs;
        TestingUtils tu;
        PasswordReset newPasswordReset;

        public ResetPasswordUT()
        {
            _db = new DatabaseContext();
            tu = new TestingUtils();
            rs = new ResetService();
        }
        
        [TestMethod]
        public void CreatePasswordReset_Pass()
        {
            //Arrange
            newPasswordReset = tu.CreatePasswordResetObject();
            var expected = newPasswordReset;
            //Act
            using(_db = tu.CreateDataBaseContext())
            {
                var response = rs.CreatePasswordReset(_db, newPasswordReset);
                _db.SaveChanges();
                //Assert
                Assert.IsNotNull(response);
                Assert.AreEqual(response.ResetToken, expected.ResetToken);
            }
        }

        [TestMethod]
        public void CreatePasswordReset_Fail()
        {
            //Arrange
            newPasswordReset = tu.CreatePasswordResetObject();
            var expected = tu.CreatePasswordResetObject();
            //Act
            using (_db = tu.CreateDataBaseContext())
            {
                var response = rs.CreatePasswordReset(_db, newPasswordReset);
                _db.SaveChanges();
                //Assert
                Assert.IsNotNull(response);
                Assert.AreNotEqual(response.ResetToken, expected.ResetToken);
            }
        }

        [TestMethod]
        public void GetPasswordReset_Pass()
        {
            //Arrange
            newPasswordReset = tu.CreatePasswordResetObject();
            var expected = newPasswordReset;
           
            //Act
            using(_db = tu.CreateDataBaseContext())
            {
                newPasswordReset = rs.CreatePasswordReset(_db, newPasswordReset);
                _db.SaveChanges();
                var result = rs.GetPasswordReset(_db, newPasswordReset.ResetToken);
                //Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(result.ResetToken, expected.ResetToken);
            }

        }

        [TestMethod]
        public void GetPasswordReset_Fail()
        {
            //Arrange
            string NonExistingResetToken = "asdf";
            PasswordReset expected = null;

            //Act
            using (_db = tu.CreateDataBaseContext())
            {
                var result = rs.GetPasswordReset(_db, NonExistingResetToken);
                //Assert
                Assert.IsNull(result);
                Assert.AreEqual(result, expected);
            }
        }

        [TestMethod]
        public void ExistingPasswordReset_Pass()
        {
            //Arrange
            newPasswordReset = tu.CreatePasswordResetObject();
            var expected = true;
            //Act
            using(_db = tu.CreateDataBaseContext())
            {
                rs.CreatePasswordReset(_db, newPasswordReset);
                _db.SaveChanges();
                var actual = rs.ExistingReset(_db, newPasswordReset.ResetToken);
                //Assert
                Assert.IsNotNull(actual);
                Assert.AreEqual(actual, expected);
            }
        }

        [TestMethod]
        public void ExistingPasswordReset_Fail()
        {
            //Arrange
            newPasswordReset = tu.CreatePasswordResetObject();
            var expected = true;
            //Act
            using (_db = tu.CreateDataBaseContext())
            {
                var actual = rs.ExistingReset(_db, newPasswordReset.ResetToken);
                //Assert
                Assert.IsNull(actual);
                Assert.AreNotEqual(actual, expected);
            }
        }

        [TestMethod]
        public void UpdatePasswordReset_Pass()
        {
            //Arrange
            newPasswordReset = tu.CreatePasswordResetObject();
            var expectedResultExpirationTime = newPasswordReset.ExpirationTime;
            var expectedResult = newPasswordReset;
            //Act
            using(_db = tu.CreateDataBaseContext())
            {
                newPasswordReset = rs.CreatePasswordReset(_db, newPasswordReset);
                _db.SaveChanges();
                newPasswordReset.ExpirationTime = DateTime.Now.AddYears(5);
                var response = rs.UpdatePasswordReset(_db, newPasswordReset);
                _db.SaveChanges();
                var result = _db.ResetIDs.Find(expectedResult.ResetToken);

                // Assert
                Assert.IsNotNull(response);
                Assert.IsNotNull(result);
                Assert.AreEqual(result.ResetToken, expectedResult.ResetToken);
                Assert.AreNotEqual(result.ExpirationTime, expectedResultExpirationTime);
            }
        }

        [TestMethod]
        public void UpdatePasswordReset_Fail()
        {
            //Arrange
            newPasswordReset = tu.CreatePasswordResetObject();
            PasswordReset expectedResult = newPasswordReset;
            //Act
            using(_db = tu.CreateDataBaseContext())
            {
                var response = rs.UpdatePasswordReset(_db, newPasswordReset);
                try
                {
                    _db.SaveChanges();
                }
                catch (Exception)
                {
                    // catch error
                    // rollback changes
                    _db.Entry(newPasswordReset).State = System.Data.Entity.EntityState.Detached;
                }
                var result = _db.ResetIDs.Find(expectedResult.ResetToken);

                // Assert
                Assert.IsNotNull(response);
                Assert.IsNull(result);
            }
        }

        [TestMethod]
        public void DeletePasswordReset_Pass()
        {
            // Arrange
            newPasswordReset = tu.CreatePasswordResetObject();

            //Act
            using(_db = tu.CreateDataBaseContext())
            {
                var expected = rs.CreatePasswordReset(_db, newPasswordReset);
                _db.SaveChanges();

                var response = rs.DeletePasswordReset(_db, expected.ResetToken);
                _db.SaveChanges();

                var result = _db.ResetIDs.Find(expected.ResetToken);

                //Assert
                Assert.IsNotNull(response);
                Assert.IsNull(result);
                Assert.AreEqual(response.ResetToken, expected.ResetToken);
            }
        }

        [TestMethod]
        public void DeletePasswordReset_Fail()
        {
            // Arrange
            string nonexistingPasswordResetToken = "asdf";
            var expected = nonexistingPasswordResetToken;

            //Act
            using (_db = tu.CreateDataBaseContext())
            {

                var response = rs.DeletePasswordReset(_db, expected);
                _db.SaveChanges();

                var result = _db.ResetIDs.Find(expected);

                //Assert
                Assert.IsNotNull(response);
                Assert.IsNull(result);
            }
        }



    }
}
