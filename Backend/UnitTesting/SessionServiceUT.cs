using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataAccessLayer.Database;
using DataAccessLayer.Models;
using ServiceLayer.Services;

namespace UnitTesting
{
    [TestClass]
    public class SessionServiceUT
    {
        SessionService ss;
        User u1;
        User u2;
        Session s1;
        TestingUtils tu;
        public SessionServiceUT()
        {
            //Arrange
            tu = new TestingUtils();
            ss = new SessionService();
            u1 = tu.CreateUserInDb();
            u2 = tu.CreateUserInDb();
            s1 = tu.CreateSession(u1);
        }

        [TestMethod]
        public void generateSession()
        {
            //Act
            string s1 = ss.GenerateSession();
            string s2 = ss.GenerateSession();
            //Assert
            Assert.AreEqual(64, s1.Length);
            Assert.AreNotEqual(s1, s2);
        }

        [TestMethod]
        public void validateSession()
        {
            Assert.AreEqual(true, ss.ValidateSession(u1));
            Assert.AreEqual(false, ss.ValidateSession(u2));
        }
    }
}
