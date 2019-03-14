using System;
using DataAccessLayer.Database;
using DataAccessLayer.Models;
using ManagerLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServiceLayer.Exceptions;

namespace UnitTesting
{
    [TestClass]
    public class UserManagerUT
    {
        TestingUtils _tu;
        DatabaseContext _db;
        UserManager _um;

        public UserManagerUT()
        {
            _tu = new TestingUtils();
            _um = new UserManager();
        }

        [TestMethod]
        public void Create_And_Get_User()
        {
            // Arrange
            string email = Guid.NewGuid() + "@" + Guid.NewGuid() + ".com";
            string password = (Guid.NewGuid()).ToString();
            DateTime dob = DateTime.Now.AddYears(-18);
            string city = "MyCity";
            string state = "MyState";
            string country = "MyCountry";
            string securityQ1 = "MySecurityQ1";
            string securityQ1Answer = "MySecurityAnswerQ1";
            string securityQ2 = "MySecurityQ2";
            string securityQ2Answer = "MySecurityAnswerQ2";
            string securityQ3 = "MySecurityQ3";
            string securityQ3Answer = "MySecurityAnswerQ3";

            // Act
            using (var _db = new DatabaseContext())
            {
                var response = _um.CreateUser(_db,
                    email,
                    password,
                    dob,
                    city,
                    state,
                    country,
                    securityQ1,
                    securityQ1Answer,
                    securityQ2,
                    securityQ2Answer,
                    securityQ3,
                    securityQ3Answer);

                var result = _um.GetUser(_db, response.Id);

                // Assert 
                Assert.IsNotNull(response);
                Assert.IsNotNull(result);
                Assert.AreEqual(email, result.Email);
            }
        }

        [TestMethod]
        public void Create_User_InvalidEmail()
        {
            // Arrange
            string email = Guid.NewGuid() + ".com";
            string password = (Guid.NewGuid()).ToString();
            DateTime dob = DateTime.Now.AddYears(-18);
            string city = "MyCity";
            string state = "MyState";
            string country = "MyCountry";
            string securityQ1 = "MySecurityQ1";
            string securityQ1Answer = "MySecurityAnswerQ1";
            string securityQ2 = "MySecurityQ2";
            string securityQ2Answer = "MySecurityAnswerQ2";
            string securityQ3 = "MySecurityQ3";
            string securityQ3Answer = "MySecurityAnswerQ3";

            // Act
            using (var _db = new DatabaseContext())
            {
                try
                {
                    var response = _um.CreateUser(_db,
                        email,
                        password,
                        dob,
                        city,
                        state,
                        country,
                        securityQ1,
                        securityQ1Answer,
                        securityQ2,
                        securityQ2Answer,
                        securityQ3,
                        securityQ3Answer);

                    throw new Exception("Test failed - Manager did not throw exception.");
                } catch (FormatException){}
            }
        }

        [TestMethod]
        public void Create_User_InsecurePassword()
        {
            // Arrange
            string email = Guid.NewGuid() + "@" + Guid.NewGuid() + ".com";
            string password = "123456789abc";
            DateTime dob = DateTime.Now.AddYears(-18);
            string city = "MyCity";
            string state = "MyState";
            string country = "MyCountry";
            string securityQ1 = "MySecurityQ1";
            string securityQ1Answer = "MySecurityAnswerQ1";
            string securityQ2 = "MySecurityQ2";
            string securityQ2Answer = "MySecurityAnswerQ2";
            string securityQ3 = "MySecurityQ3";
            string securityQ3Answer = "MySecurityAnswerQ3";

            // Act
            using (var _db = new DatabaseContext())
            {
                try
                {
                    var response = _um.CreateUser(_db,
                        email,
                        password,
                        dob,
                        city,
                        state,
                        country,
                        securityQ1,
                        securityQ1Answer,
                        securityQ2,
                        securityQ2Answer,
                        securityQ3,
                        securityQ3Answer);

                    throw new Exception("Test failed - Manager did not throw exception.");
                }
                catch (PasswordPwnedException) { }
            }
        }

        [TestMethod]
        public void Create_User_ShortPassword()
        {
            // Arrange
            string email = Guid.NewGuid() + "@" + Guid.NewGuid() + ".com";
            string password = "123456";
            DateTime dob = DateTime.Now.AddYears(-18);
            string city = "MyCity";
            string state = "MyState";
            string country = "MyCountry";
            string securityQ1 = "MySecurityQ1";
            string securityQ1Answer = "MySecurityAnswerQ1";
            string securityQ2 = "MySecurityQ2";
            string securityQ2Answer = "MySecurityAnswerQ2";
            string securityQ3 = "MySecurityQ3";
            string securityQ3Answer = "MySecurityAnswerQ3";

            // Act
            using (var _db = new DatabaseContext())
            {
                try
                {
                    var response = _um.CreateUser(_db,
                        email,
                        password,
                        dob,
                        city,
                        state,
                        country,
                        securityQ1,
                        securityQ1Answer,
                        securityQ2,
                        securityQ2Answer,
                        securityQ3,
                        securityQ3Answer);

                    throw new Exception("Test failed - Manager did not throw exception.");
                }
                catch (PasswordInvalidException) { }
            }
        }

        [TestMethod]
        public void Create_User_TooYoung()
        {
            // Arrange
            string email = Guid.NewGuid() + "@" + Guid.NewGuid() + ".com";
            string password = "123456789abc";
            DateTime dob = DateTime.Now;
            string city = "MyCity";
            string state = "MyState";
            string country = "MyCountry";
            string securityQ1 = "MySecurityQ1";
            string securityQ1Answer = "MySecurityAnswerQ1";
            string securityQ2 = "MySecurityQ2";
            string securityQ2Answer = "MySecurityAnswerQ2";
            string securityQ3 = "MySecurityQ3";
            string securityQ3Answer = "MySecurityAnswerQ3";

            // Act
            using (var _db = new DatabaseContext())
            {
                try
                {
                    var response = _um.CreateUser(_db,
                        email,
                        password,
                        dob,
                        city,
                        state,
                        country,
                        securityQ1,
                        securityQ1Answer,
                        securityQ2,
                        securityQ2Answer,
                        securityQ3,
                        securityQ3Answer);

                    throw new Exception("Test failed - Manager did not throw exception.");
                }
                catch (PasswordInvalidException) { }
            }
        }

        [TestMethod]
        public void Create_User_DuplicateEmail()
        {
            User existingUser = _tu.CreateUserInDb();
            // Arrange
            string email = existingUser.Email;
            string password = (Guid.NewGuid()).ToString();
            DateTime dob = DateTime.Now.AddYears(-18);
            string city = "MyCity";
            string state = "MyState";
            string country = "MyCountry";
            string securityQ1 = "MySecurityQ1";
            string securityQ1Answer = "MySecurityAnswerQ1";
            string securityQ2 = "MySecurityQ2";
            string securityQ2Answer = "MySecurityAnswerQ2";
            string securityQ3 = "MySecurityQ3";
            string securityQ3Answer = "MySecurityAnswerQ3";

            // Act
            using (var _db = new DatabaseContext())
            {
                try
                {
                    var response = _um.CreateUser(_db,
                        email,
                        password,
                        dob,
                        city,
                        state,
                        country,
                        securityQ1,
                        securityQ1Answer,
                        securityQ2,
                        securityQ2Answer,
                        securityQ3,
                        securityQ3Answer);

                    throw new Exception("Test failed - Manager did not throw exception.");
                } catch (ArgumentException) {}
            }
        }
    }
}
