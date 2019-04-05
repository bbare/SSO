using System;
using System.Data.Entity.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataAccessLayer.Database;
using DataAccessLayer.Models;
using ServiceLayer.Services;
using ManagerLayer;

namespace UnitTesting
{
	[TestClass]
	public class SessionTimeOutUT
	{
		[TestMethod]
		public void Session_Time_Out_Pass()
		{
			
			//Arrange
			DatabaseContext _db = new DatabaseContext();
			TestingUtils tu = new TestingUtils();
			SessionService ss = new SessionService();
			AuthorizationManager _am;
			User newUser1 = tu.CreateUserObject();
			User newUser2 = tu.CreateUserObject();
			Session ValidSession;
			Session ExpiredSession;
			//Act
			_am = new AuthorizationManager();
			newUser1 = tu.CreateUserInDb();
			newUser2 = tu.CreateUserInDb();
			ValidSession = tu.CreateSessionInDb(newUser1);
			ExpiredSession = tu.CreateExpiredSessionInDb(newUser2);
			//Assert
			Assert.IsNull(_am.ValidateAndUpdateSession(_db,ExpiredSession.Token));
			/*
			DatabaseContext _db = new DatabaseContext();
			SessionService ss = new SessionService();
			ss.DeleteSession(_db, "6d5ff034-cb8d-483d-be5f-b36c9a37c9bb");
			ss.DeleteSession(_db, "4b42ac4b-03d1-4efc-b3a9-20cdf872b715");
			ss.DeleteSession(_db, "f95fd82a-9301-4b6f-a659-0ff7a149e83e");
			ss.DeleteSession(_db, "89183b1b-5801-4f85-84d1-0ee244e7cca6");
			ss.DeleteSession(_db, "8d2a1100-96aa-460a-bab2-c234194ca573");
			ss.DeleteSession(_db, "82df12c682f18fcf292bd16bc8d7df74ad50553ce005a095a42912afc8f5ac01");
			ss.DeleteSession(_db, "d7d1d9b8-ca6e-418c-8f98-2bad219884f2");
			ss.DeleteSession(_db, "34a85553-5399-4bc3-a8cd-569e71f1b3a1");
			ss.DeleteSession(_db, "43f2d44cb6b46af4e82a5bafc62041ac91953aa4666dc546bf2882293f7a6162");
			*/

		}
	}
}
