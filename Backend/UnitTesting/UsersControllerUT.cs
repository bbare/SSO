using System;
using System.Text;
using System.Collections.Generic;
using KFC_WebAPI.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTesting
{
    [TestClass]
    public class UsersControllerUT
    {

        UsersController uc;
        public UsersControllerUT()
        {
            uc = new UsersController();
        }

        //Login()

        [TestMethod]
        public void Login_Success_ReturnNotFound()
        {

        }

        [TestMethod]
        public void Login_Success_ReturnUnauthorized()
        {

        }

        [TestMethod]
        public void Login_Success_ReturnOk()
        {

        }
    }
}
