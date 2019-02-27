using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceLayer.Services;

namespace UnitTesting
{
    [TestClass]
    class ResetPasswordUT
    {
        [TestMethod]
        public void createToken_Pass()
        {
            //Arrange
            string resetToken = "asdf";
            ResetService _resetService = new ResetService();
            var expected = resetToken;
            //Act
            var actual = _resetService.createToken("foo@bar.com");
            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void createToken_Fail()
        {
            //Arrange
            string resetToken = "asdf";
            ResetService _resetService = new ResetService();
            var expected = resetToken;
            //Act
            var actual = _resetService.createToken("foo@bar.com");
            //Assert
            Assert.AreNotEqual(expected, actual);
        }

        [TestMethod]
        public void createURL_Pass()
        {

        }

        [TestMethod]
        public void createURL_Fail()
        {
            //Arrange
            string resetURL = "kfcsso.com/asdf";
        }

        [TestMethod]
        public void expiredToken_Pass()
        {

        }
        [TestMethod]
        public void expiredToken_Fail()
        {

        }

        
        

    }
}
