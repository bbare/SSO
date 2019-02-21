using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServiceLayer.Services;

namespace UnitTesting
{
    /// <summary>
    /// Unit testing for Password Service.
    /// Tests included: Hashing methods and pwned password api service
    /// </summary>
    [TestClass]
    public class PasswordServiceUT
    {
        PasswordService ps;
        TestingUtils tu;
        public PasswordServiceUT()
        {
            //Arrange
            ps = new PasswordService();
            tu = new TestingUtils();
        }

        [TestMethod]
        public void hashPassword()
        {
            //Act
            string password = "buasd78324yas";
            byte[] salt = ps.GenerateSalt();
            string hash1 = ps.HashPassword(password, salt);
            string hash2 = ps.HashPassword(password, salt);

            password = "uibava97s133";
            string hash3 = ps.HashPassword(password, salt);
            


            //Assert
            Assert.AreEqual(hash1, hash2);
            Assert.AreNotEqual(hash1, hash3);
        }

        [TestMethod]
        public void HashPasswordSHA1()
        {
            //Act
            string password = "password";
           
            string hash1 = ps.HashPasswordSHA1(password, null);
            string hash2 = ps.HashPasswordSHA1(password, null);
            string hashed1 = "5BAA61E4C9B93F3F0682250B6CF8331B7EE68FD8";
            password = "12t3h0eu93h9ke";
            
            string hash3 = ps.HashPasswordSHA1(password, null);
            //Assert
            Assert.AreEqual(hash1, hashed1);
            Assert.AreEqual(hash1, hash2);
            Assert.AreNotEqual(hash1, hash3);
        }

        [TestMethod]
        public void CheckPasswordPwned()
        {

            Assert.AreNotEqual(0, ps.CheckPasswordPwned("password"));
            Assert.AreEqual(0, ps.CheckPasswordPwned("ASDfas!@fdasf!223gs3"));
            
        }

        [TestMethod]
        public void QueryPwnedApi()
        {
            //Act
            string url = "https://api.pwnedpasswords.com/range/";
            string url_broken = "";
            string prefix = ps.HashPasswordSHA1("password", null).Substring(0, 5);
            string prefix2 = ps.HashPasswordSHA1("letgooooo", null).Substring(0, 5);

            //Assert
            Assert.ThrowsException<InvalidOperationException>(() => ps.QueryPwnedApi(prefix, url_broken));

            Assert.AreEqual(true, tu.isEqual(ps.QueryPwnedApi(prefix, url), ps.QueryPwnedApi(prefix, url)));
            Assert.AreEqual(false, tu.isEqual(ps.QueryPwnedApi(prefix, url), ps.QueryPwnedApi(prefix2, url)));

        }

       
    }
}
