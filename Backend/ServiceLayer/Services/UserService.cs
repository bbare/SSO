using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using DataAccessLayer.Database;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories;

namespace ServiceLayer.Services
{
    public class UserService : IUserService
    {
        private UserManagementRepository _UserManagementRepo;

        public UserService()
        {
            _UserManagementRepo = new UserManagementRepository();
        }

        public User CreateUser(DatabaseContext _db, User user)
        {
            if (_UserManagementRepo.ExistingUser(_db, user))
            {
                Console.WriteLine("User exists");
                return null;
            }
            return _UserManagementRepo.CreateNewUser(_db, user);
        }

        public User DeleteUser(DatabaseContext _db, Guid Id)
        {
            return _UserManagementRepo.DeleteUser(_db, Id);
        }

        public User GetUser(DatabaseContext _db, string email)
        {
            return _UserManagementRepo.GetUser(_db, email);
        }

        public User GetUser(DatabaseContext _db, Guid Id)
        {
            return _UserManagementRepo.GetUser(_db, Id);
        }

        public User UpdateUser(DatabaseContext _db, User user)
        {
            return _UserManagementRepo.UpdateUser(_db, user);
        }

        public bool IsManagerOver(DatabaseContext _db, User user, User subject)
        {
            return _UserManagementRepo.IsManagerOver(_db, user, subject);
        }

        public User Login(DatabaseContext _db, string email, string password)
        {
            UserRepository userRepo = new UserRepository();
            PasswordService _passwordService = new PasswordService();
            var user = _UserManagementRepo.GetUser(_db, email);
            if (user != null)
            {
                string hashedPassword = _passwordService.HashPassword(password, user.PasswordSalt);
                if (userRepo.ValidatePassword(user, hashedPassword))
                {
                    Console.WriteLine("Password Correct");
                    return user;
                }
                Console.WriteLine("Password Incorrect");
                return null;
            }
            Console.WriteLine("User does not exist");
            return null;
        }
    }
}
