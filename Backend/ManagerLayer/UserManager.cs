using DataAccessLayer.Database;
using DataAccessLayer.Models;
using ServiceLayer.Exceptions;
using ServiceLayer.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace ManagerLayer
{
    public class UserManager
    {
        public User CreateUser(
            DatabaseContext _db,
            string email,
            string password,
            DateTime dob,
            string city,
            string state,
            string country,
            string securityQ1,
            string securityQ1Answer,
            string securityQ2,
            string securityQ2Answer,
            string securityQ3,
            string securityQ3Answer)
        {
            new System.Net.Mail.MailAddress(email);

            DateTime today18YearsAgo = DateTime.Now.AddYears(-18);
            if (dob > today18YearsAgo)
            {
                throw new InvalidDobException("Date of birth less than 18 years ago");
            }

            IPasswordService _passwordService = new PasswordService();

            if (!_passwordService.CheckPasswordLength(password))
            {
                throw new PasswordInvalidException("Password is too short");
            }

            int pwnedCount = _passwordService.CheckPasswordPwned(password);

            if (pwnedCount > 0)
            {
                throw new PasswordPwnedException("Password has been pwned");
            }

            byte[] salt = _passwordService.GenerateSalt();
            string hash = _passwordService.HashPassword(password, salt);

            User user = new User
            {
                Email = email,
                PasswordHash = hash,
                PasswordSalt = salt,

                DateOfBirth = dob,
                City = city,
                State = state,
                Country = country,

                SecurityQ1 = securityQ1,
                SecurityQ1Answer = securityQ1Answer,
                SecurityQ2 = securityQ2,
                SecurityQ2Answer = securityQ2Answer,
                SecurityQ3 = securityQ3,
                SecurityQ3Answer = securityQ3Answer,
                UpdatedAt = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow
            };

            IUserService _userService = new UserService();
            return _userService.CreateUser(_db, user);
        }


        /// <summary>
        ///     Not part of spring 3
        ///     - in development
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        public void Login(string email, string password)
        {
            //UserService _userService = new UserService();
            //PasswordService _passwordService = new PasswordService();
            //var user = _userService.Login(email, password);
        }

        public User GetUser(DatabaseContext _db, Guid userId)
        {
            IUserService _userService = new UserService();
            return _userService.GetUser(_db, userId);
        }

        /// <summary>
        ///     Delete user from SSO
        ///     - in development
        /// </summary>
        public User DeleteUser(DatabaseContext _db, Guid userId)
        {
            // IUserDeleteService
            UserDeleteService udrs = new UserDeleteService();
            IUserService _userService = new UserService();
            try
            {

            }
            catch
            {


            }
            return _userService.DeleteUser(_db, userId);
        }
    }
}
