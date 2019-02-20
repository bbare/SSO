using ManagerLayer;
using ManagerLayer.UserManagement;
using System;
<<<<<<< HEAD
using DataAccessLayer.Database;
using ServiceLayer.Services;
using DataAccessLayer.Models;
using UnitTesting;
=======
>>>>>>> 9de364cebb612877c7f3127cd8a6d9c1f7839428

namespace Program
{
    class Program
    {
        static void Main(string[] args)
        {
<<<<<<< HEAD
            var tu = new TestingUtils();
            var newUser = tu.CreateUserObject();
            Console.WriteLine(newUser.Email);
            using (var _db = new DatabaseContext())
            {
                var us = new UserService();
                us.CreateUser(_db, newUser);
                _db.SaveChanges();
            }
=======

            // create user
            // the business req's to crete a user should go in the manger
            //  - call services to create user and add to the database

//            UserManagementManager umm = new UserManagementManager();
  //          Guid guid = new Guid("f8d0c634-159e-4e8a-a561-19bc118a1b49");
    //        UserManager userManger = new UserManager();
      //      umm.DeleteUser(guid);
>>>>>>> 9de364cebb612877c7f3127cd8a6d9c1f7839428
            Console.ReadKey();
        }
    }
}
