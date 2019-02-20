using ManagerLayer;
using ManagerLayer.UserManagement;
using System;
using DataAccessLayer.Database;
using ServiceLayer.Services;
using DataAccessLayer.Models;
using UnitTesting;

namespace Program
{
    class Program
    {
        static void Main(string[] args)
        {
            var tu = new TestingUtils();
            var newUser = tu.CreateUserObject();
            Console.WriteLine(newUser.Email);
            using (var _db = new DatabaseContext())
            {
                var us = new UserService();
                us.CreateUser(_db, newUser);
                _db.SaveChanges();
            }
            Console.ReadKey();
        }
    }
}
