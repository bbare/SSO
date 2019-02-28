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
        public void createResetID_Pass()
        {
            //Arrange
            int lengthOfID = 18;
            ResetService _resetService = new ResetService();
            var expected = lengthOfID;
            //Act
            string resetIDCreated = _resetService.createResetID();
            var actual = resetIDCreated.Length;
            //Assert
            Assert.IsTrue(actual > expected);
        }

        [TestMethod]
        public void createToken_Fail()
        {
            //Arrange
            int lengthOfID = 22;
            ResetService _resetService = new ResetService();
            var expected = lengthOfID;
            //Act
            string resetIDCreated = _resetService.createResetID();
            var actual = resetIDCreated.Length;
            //Assert
            Assert.IsFalse(actual > expected);
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
