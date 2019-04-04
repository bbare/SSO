using System;
using System.Data.Entity.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataAccessLayer.Database;
using DataAccessLayer.Models;
using ServiceLayer.Services;

namespace UnitTesting
{
	[TestClass]
	class SessionTimeOutUT
	{
		[TestMethod]
		public void Session_Time_Out_Pass()
		{
			//Arrange
			DatabaseContext _db = new DatabaseContext();
			TestingUtils tu = new TestingUtils();
			SessionService ss = new SessionService();
			User newUser = tu.CreateUserObject();
			Session newSession;
			TestingUtils tu;
			//Act
			//Assert
		}
	}
}
