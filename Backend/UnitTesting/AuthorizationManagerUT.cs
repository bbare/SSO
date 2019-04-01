using System;
using DataAccessLayer.Database;
using DataAccessLayer.Models;
using ManagerLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServiceLayer.Exceptions;

namespace UnitTesting
{
    [TestClass]
    public class AuthorizationManagerUT
    {
        TestingUtils _tu;
        DatabaseContext _db;
        AuthorizationManager _am;

        public AuthorizationManagerUT()
        {
            _tu = new TestingUtils();
            _am = new AuthorizationManager();
        }

        [TestMethod]
        public void GenerateSession()
        {
            string sessionToken1 = _am.GenerateSessionToken();
            string sessionToken2 = _am.GenerateSessionToken();

            Assert.AreEqual(sessionToken1.Length, 64);
            Assert.AreNotEqual(sessionToken1, sessionToken2);
        }

        [TestMethod]
        public void Create_And_Validate_Token()
        {
            // Arrange
            User user = _tu.CreateUserInDb();

            // Act
            using (var _db = new DatabaseContext())
            {
                Session session = _am.CreateSession(_db, user);

                _db.SaveChanges();

                Session validatedSession = _am.ValidateAndUpdateSession(_db, session.Token);

                // Assert 
                Assert.IsNotNull(validatedSession);
                Assert.AreEqual(session.Token, validatedSession.Token);
                Assert.AreEqual(session.Id, validatedSession.Id);
            }
        }

        [TestMethod]
        public void Validate_Invalid_Token()
        {
            using (var _db = new DatabaseContext())
            {
                Session validatedSession = _am.ValidateAndUpdateSession(_db, "invalidToken");
                
                Assert.IsNull(validatedSession);
            }
        }

        [TestMethod]
        public void Create_And_Delete_Token()
        {
            // Arrange
            User user = _tu.CreateUserInDb();
            Session session = _tu.CreateSessionInDb(user);

            // Act
            using (var _db = new DatabaseContext())
            {
                Session deletedSession = _am.DeleteSession(_db, session.Token);

                _db.SaveChanges();

                Session validatedSession = _am.ValidateAndUpdateSession(_db, session.Token);

                // Assert 
                Assert.IsNotNull(deletedSession);
                Assert.AreEqual(session.Token, deletedSession.Token);
                Assert.AreEqual(session.Id, deletedSession.Id);
                Assert.IsNull(validatedSession);
            }
        }
    }
}
