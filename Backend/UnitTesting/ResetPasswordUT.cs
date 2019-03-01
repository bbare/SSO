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
        public void CreateResetID_Pass()
        {
            //Arrange
            ResetService _resetService = new ResetService();
            var expected = 17;
            //Act
            string resetIDCreated = _resetService.CreateResetID();
            var actual = resetIDCreated.Length;
            //Assert
            Assert.IsTrue(actual > expected);
        }

        [TestMethod]
        public void CreateResetID_Fail()
        {
            //Arrange
            ResetService _resetService = new ResetService();
            var expected = 21;
            //Act
            string resetIDCreated = _resetService.CreateResetID();
            var actual = resetIDCreated.Length;
            //Assert
            Assert.IsFalse(actual > expected);
        }
    }
}
