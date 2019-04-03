using DataAccessLayer.Database;
using ManagerLayer.PasswordManagement;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace UnitTesting
{
    [TestClass]
    public class PasswordManagerUT
    {
        DatabaseContext _db;
        TestingUtils tu;
        PasswordManager pm;

        public PasswordManagerUT()
        {
            _db = new DatabaseContext();
            tu = new TestingUtils();
            pm = new PasswordManager();
        }

        [TestMethod]
        public void CreatePasswordReset_Pass()
        {
            //Arrange
            var newUser = tu.CreateUserObject();
            tu.CreateUserInDb(newUser);
            //Act
            var response = pm.CreatePasswordReset(newUser.Id);
            //Assert
            Assert.IsNotNull(response);
        }

        [TestMethod]
        public void GetPasswordReset_Pass()
        {
            //Arrange
            var newUser = tu.CreateUserObject();
            tu.CreateUserInDb(newUser);
            var expected = pm.CreatePasswordReset(newUser.Id);
            //Act
            var response = pm.GetPasswordReset(expected.ResetToken);
            //Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(expected.ResetToken, response.ResetToken);
        }

        [TestMethod]
        public void UpdatePasswordReset_Pass()
        {
            //Arrange
            var newUser = tu.CreateUserObject();
            tu.CreateUserInDb(newUser);
            var expected = pm.CreatePasswordReset(newUser.Id);
            //Act
            expected.ResetCount = 1;
            var response = pm.UpdatePasswordReset(expected);
            var actual = pm.GetPasswordReset(expected.ResetToken);
            //Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(expected.ResetToken, actual.ResetToken);
        }

        [TestMethod]
        public void DeletePasswordReset_Pass()
        {
            //Arrange
            var newUser = tu.CreateUserObject();
            tu.CreateUserInDb(newUser);
            var expected = pm.CreatePasswordReset(newUser.Id);
            //Act
            var response = pm.DeletePasswordReset(expected.ResetToken);
            var result = pm.ExistingResetToken(expected.ResetToken);
            //Assert
            Assert.IsNotNull(response);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void GetPasswordResetExpiration_Pass()
        {
            //Arrange
            var newUser = tu.CreateUserObject();
            tu.CreateUserInDb(newUser);
            var expected = pm.CreatePasswordReset(newUser.Id);
            //Act
            var response = pm.GetPasswordResetExpiration(expected.ResetToken);
            //Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(expected.ExpirationTime, response);
        }

        [TestMethod]
        public void CheckExistingPasswordReset_Pass()
        {
            //Arrange
            var newUser = tu.CreateUserObject();
            tu.CreateUserInDb(newUser);
            var newlyAddedPasswordReset = pm.CreatePasswordReset(newUser.Id);
            //Act
            var response = pm.ExistingResetToken(newlyAddedPasswordReset.ResetToken);
            //Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response);
        }

        [TestMethod]
        public void GetAttemptsPerID_Pass()
        {
            //Arrange
            var newUser = tu.CreateUserObject();
            tu.CreateUserInDb(newUser);
            var newlyAddedPasswordReset = pm.CreatePasswordReset(newUser.Id);
            //Act
            var response = pm.GetAttemptsPerID(newlyAddedPasswordReset.ResetToken);
            //Assert
            Assert.IsNotNull(response);
        }

        [TestMethod]
        public void GetPasswordResetStatus()
        {
            //Arrange
            var newUser = tu.CreateUserObject();
            tu.CreateUserInDb(newUser);
            var newlyAddedPasswordReset = pm.CreatePasswordReset(newUser.Id);
            //Act
            var response = pm.GetPasswordResetStatus(newlyAddedPasswordReset.ResetToken);
            //Assert
            Assert.IsNotNull(response);
        }

        [TestMethod]
        public void LockPasswordReset_Pass()
        {
            //Arrange
            var newUser = tu.CreateUserObject();
            tu.CreateUserInDb(newUser);
            var newlyAddedPasswordReset = pm.CreatePasswordReset(newUser.Id);
            //Act
            pm.LockPasswordReset(newlyAddedPasswordReset.ResetToken);
            var response = pm.GetPasswordReset(newlyAddedPasswordReset.ResetToken);
            //Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.Disabled);
            Assert.IsFalse(response.AllowPasswordReset);
        }

        [TestMethod]
        public void CheckPasswordResetValid_Pass()
        {
            //Arrange
            var newUser = tu.CreateUserObject();
            tu.CreateUserInDb(newUser);
            var newlyAddedPasswordReset = pm.CreatePasswordReset(newUser.Id);
            //Act
            var response = pm.CheckPasswordResetValid(newlyAddedPasswordReset.ResetToken);
            //Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response);
        }

        [TestMethod]
        public void CountPasswordResetPast24Hours_Pass()
        {
            //Arrange
            var newUser = tu.CreateUserObject();
            tu.CreateUserInDb(newUser);
            var newlyAddedPasswordReset = pm.CreatePasswordReset(newUser.Id);
            //Act
            var response = pm.PasswordResetsMadeInPast24HoursByUser(newUser.Id);
            //Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(response, 1);
        }

        [TestMethod]
        public void CheckPasswordPwned_Pass()
        {
            //Arrange
            string password = "password";
            //Act
            var response = pm.CheckIsPasswordPwned(password);
            //Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response);
        }

        [TestMethod]
        public void SaltAndHashPassword_Pass()
        {
            //Arrange
            string password = "password";
            var user = tu.CreateUserObject();
            string resetToken = tu.CreatePasswordResetObject(user).ResetToken;
            //Act
            var response = pm.SaltAndHashPassword(resetToken, password);
            //Assert
            Assert.IsNotNull(response);
            Assert.AreNotEqual(response, password);
        }

        [TestMethod]
        public void UpdatePasswordLoggedIn_Pass()
        {
            //Arrange
            var newUser = tu.CreateUserObject();
            tu.CreateUserInDb(newUser);
            string newPassword = "asdf";
            //Act
            var response = pm.UpdatePassword(newUser, newPassword);
            var actual = newUser.PasswordHash;
            //Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response);
            Assert.AreEqual(actual, newPassword);
        }

        [TestMethod]
        public void UpdatePasswordNotLoggedIn_Pass()
        {
            //Arrange
            var newUser = tu.CreateUserObject();
            tu.CreateUserInDb(newUser);
            var newlyAddedPasswordReset = pm.CreatePasswordReset(newUser.Id);
            string newPassword = "asdf";
            //Act
            var response = pm.UpdatePassword(newlyAddedPasswordReset.ResetToken, newPassword);
            var actual = _db.Users.Find(newUser.Id).PasswordHash;
            //Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response);
            Assert.AreEqual(actual, newPassword);
        }

        [TestMethod]
        public void GetSecurityQuestions_Pass()
        {
            //Arrange
            var newUser = tu.CreateUserObject();
            string secQ1 = "Favorite food?";
            string secQ2 = "Favorite color?";
            string secQ3 = "Favorite hobby?";
            newUser.SecurityQ1 = secQ1;
            newUser.SecurityQ2 = secQ2;
            newUser.SecurityQ3 = secQ3;
            tu.CreateUserInDb(newUser);
            var newlyAddedPasswordReset = pm.CreatePasswordReset(newUser.Id);
            //Act 
            var response = pm.GetSecurityQuestions(newlyAddedPasswordReset.ResetToken);
            //Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(response[0], secQ1);
            Assert.AreEqual(response[1], secQ2);
            Assert.AreEqual(response[2], secQ3);
        }

        [TestMethod]
        public void CheckSecurityAnswers_Pass()
        {
            //Arrange
            var newUser = tu.CreateUserObject();
            string secA1 = "Pizza";
            string secA2 = "Cyan";
            string secA3 = "Hiking";
            newUser.SecurityQ1Answer = secA1;
            newUser.SecurityQ2Answer = secA2;
            newUser.SecurityQ3Answer = secA3;
            tu.CreateUserInDb(newUser);
            var newlyAddedPasswordReset = pm.CreatePasswordReset(newUser.Id);
            List<string> submittedAnswers = new List<string>
            {
                "Pizza",
                "Cyan",
                "Hiking"
            };
            //Act
            var response = pm.CheckSecurityAnswers(newlyAddedPasswordReset.ResetToken, submittedAnswers);
            //Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response);
        }

        [TestMethod]
        public void CheckIfPasswordResetAllowed_Pass()
        {
            //Arrange
            var newUser = tu.CreateUserObject();
            tu.CreateUserInDb(newUser);
            var newlyAddedPasswordReset = pm.CreatePasswordReset(newUser.Id);
            newlyAddedPasswordReset.AllowPasswordReset = true;
            pm.UpdatePasswordReset(newlyAddedPasswordReset);
            //Act
            var response = pm.CheckIfPasswordResetAllowed(newlyAddedPasswordReset.ResetToken);
            //Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response);
        }



        [TestMethod]
        public void CreatePasswordReset_Fail()
        {
            //Arrange
            var newUser = tu.CreateUserObject();
            tu.CreateUserInDb(newUser);
            //Act
            var expectedResetToken = "asdf";
            var actualResetToken = pm.CreatePasswordReset(newUser.Id);
            //Assert
            Assert.AreNotEqual(expectedResetToken, actualResetToken);
        }

        [TestMethod]
        public void GetPasswordReset_Fail()
        {
            //Arrange
            var newUser = tu.CreateUserObject();
            tu.CreateUserInDb(newUser);
            var expected = pm.CreatePasswordReset(newUser.Id);
            var unexpected = pm.CreatePasswordReset(newUser.Id);
            //Act
            var response = pm.GetPasswordReset(unexpected.ResetToken);
            //Assert
            Assert.IsNotNull(response);
            Assert.AreNotEqual(expected.ResetToken, response.ResetToken);
        }

        [TestMethod]
        public void UpdatePasswordReset_Fail()
        {
            //Arrange
            var newUser = tu.CreateUserObject();
            tu.CreateUserInDb(newUser);
            var expected = pm.CreatePasswordReset(newUser.Id);
            //Act
            var response = pm.UpdatePasswordReset(expected);
            var actual = pm.GetPasswordReset(expected.ResetToken);
            //Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(expected.ResetToken, actual.ResetToken);
        }

        [TestMethod]
        public void DeletePasswordReset_Fail_NonexistingToken()
        {
            //Arrange
            var newUser = tu.CreateUserObject();
            tu.CreateUserInDb(newUser);
            string resetToken = "fakeResetToken";
            //Act
            var response = pm.DeletePasswordReset(resetToken);
            var result = pm.ExistingResetToken(resetToken);
            //Assert
            Assert.IsNotNull(response);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void GetPasswordResetExpiration_Fail_NonexistingToken()
        {
            //Arrange
            var newUser = tu.CreateUserObject();
            tu.CreateUserInDb(newUser);
            string resetToken = "fakeResetToken";
            //Act
            var response = pm.GetPasswordResetExpiration(resetToken);
            //Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(DateTime.MinValue, response);
        }

        [TestMethod]
        public void CheckExistingPasswordReset_Fail()
        {
            //Arrange
            var newUser = tu.CreateUserObject();
            tu.CreateUserInDb(newUser);
            string resetToken = "fakeResetToken";
            //Act
            var response = pm.ExistingResetToken(resetToken);
            //Assert
            Assert.IsNotNull(response);
            Assert.IsFalse(response);
        }

        [TestMethod]
        public void GetPasswordResetStatus_Fail()
        {
            //Arrange
            var newUser = tu.CreateUserObject();
            tu.CreateUserInDb(newUser);
            var newlyAddedPasswordReset = pm.CreatePasswordReset(newUser.Id);
            //Act
            var response = pm.GetPasswordResetStatus(newlyAddedPasswordReset.ResetToken);
            //Assert
            Assert.IsNotNull(response);
        }

        [TestMethod]
        public void LockPasswordReset_Fail()
        {
            //Arrange
            var newUser = tu.CreateUserObject();
            tu.CreateUserInDb(newUser);
            string resetToken = "fakeResetToken";
            //Act
            var response = pm.GetPasswordReset(resetToken);
            //Assert
            Assert.IsNull(response);
        }

        [TestMethod]
        public void UpdatePasswordNotLoggedIn_Fail()
        {
            //Arrange
            var newUser = tu.CreateUserObject();
            tu.CreateUserInDb(newUser);
            string resetToken = "fakeResetRoken";
            string newPassword = "asdf";
            //Act
            var response = pm.UpdatePassword(resetToken, newPassword);
            var actual = _db.Users.Find(newUser.Id).PasswordHash;
            //Assert
            Assert.IsNotNull(response);
            Assert.IsFalse(response);
        }

        [TestMethod]
        public void GetSecurityQuestions_Fail()
        {
            //Arrange
            var newUser = tu.CreateUserObject();

            string secQ1 = "Favorite food?";
            string secQ2 = "Favorite color?";
            string secQ3 = "Favorite hobby?";
            newUser.SecurityQ1 = secQ1;
            newUser.SecurityQ2 = secQ2;
            newUser.SecurityQ3 = secQ3;
            tu.CreateUserInDb(newUser);
            string expectedSecQ1 = "Favorite drink?";
            string expectedSecQ2 = "Favorite pantone?";
            string expectedSecQ3 = "Favorite thing to do?";
            var newlyAddedPasswordReset = pm.CreatePasswordReset(newUser.Id);
            //Act 
            var response = pm.GetSecurityQuestions(newlyAddedPasswordReset.ResetToken);
            //Assert
            Assert.IsNotNull(response);
            Assert.AreNotEqual(response[0], expectedSecQ1);
            Assert.AreNotEqual(response[1], expectedSecQ2);
            Assert.AreNotEqual(response[2], expectedSecQ3);
        }

        [TestMethod]
        public void CheckSecurityAnswers_Fail()
        {
            //Arrange
            var newUser = tu.CreateUserObject();
            string secA1 = "Pizza";
            string secA2 = "Cyan";
            string secA3 = "Hiking";
            newUser.SecurityQ1Answer = secA1;
            newUser.SecurityQ2Answer = secA2;
            newUser.SecurityQ3Answer = secA3;
            tu.CreateUserInDb(newUser);
            var newlyAddedPasswordReset = pm.CreatePasswordReset(newUser.Id);
            List<string> submittedAnswers = new List<string>
            {
                "Pizza",
                "Cyan",
                "Photography"
            };
            //Act
            var response = pm.CheckSecurityAnswers(newlyAddedPasswordReset.ResetToken, submittedAnswers);
            //Assert
            Assert.IsNotNull(response);
            Assert.IsFalse(response);
        }

        [TestMethod]
        public void CheckIfPasswordResetAllowed_Fail()
        {
            //Arrange
            var newUser = tu.CreateUserObject();
            tu.CreateUserInDb(newUser);
            var newlyAddedPasswordReset = pm.CreatePasswordReset(newUser.Id);
            newlyAddedPasswordReset.AllowPasswordReset = false;
            pm.UpdatePasswordReset(newlyAddedPasswordReset);
            //Act
            var response = pm.CheckIfPasswordResetAllowed(newlyAddedPasswordReset.ResetToken);
            //Assert
            Assert.IsNotNull(response);
            Assert.IsFalse(response);
        }
    }
}
