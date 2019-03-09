using DataAccessLayer.Database;
using DataAccessLayer.Models;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServiceLayer.Services;

using System.Security.Cryptography;

namespace UnitTesting
{
    public class TestingUtils
    {
        public byte[] GetRandomness()
        {
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }

        public User CreateUserInDb()
        {
            var q1 = "How are you?";
            var a1 = "cool";
            var q2 = "How old are you?";
            var a2 = "22";
            var q3 = "Are you sad?";
            var a3 = "no";

            User u = new User
            {
                Id = Guid.NewGuid(),
                Email = "cf2080@gmail.com",
                DateOfBirth = new DateTime(1996, 12, 15),
                City = "Long Beach",
                State = "CA",
                Country = "USA",
                PasswordHash = "d33168c3b55ddbaf34f3ff64c1047f6c605c773a",
                PasswordSalt = null,
                SecurityQ1 = q1,
                SecurityQ1Answer = a1,
                SecurityQ2 = q2,
                SecurityQ2Answer = a2,
                SecurityQ3 = q3,
                SecurityQ3Answer = a3,
                IncorrectPasswordCount = 0,
                Disabled = false,
                UpdatedAt = new DateTime(2019, 3, 6),
                CreatedAt = new DateTime(2019, 3, 6)
            };

            return CreateUserInDb(u);
        }

        public User CreateUserInDb(User user)
        {
            using (var _db = new DatabaseContext())
            {
                _db.Entry(user).State = System.Data.Entity.EntityState.Added;
                _db.SaveChanges();

                return user;
            }
        }

        public User CreateUserObject()
        {

            User user = new User
            {
                Id = Guid.NewGuid(),
                Email = Guid.NewGuid() + "@" + Guid.NewGuid() + ".com",
                DateOfBirth = DateTime.UtcNow,
                City = "Los Angeles",
                State = "California",
                Country = "United States",
                PasswordHash = (Guid.NewGuid()).ToString(),
                PasswordSalt = GetRandomness(),
                IncorrectPasswordCount = 0,
                Disabled = false
                
            };
            return user;
        }
        
        public Session CreateSessionObject(User user)
        {
            Session session = new Session
            {
                Id = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow,
                UserId = user.Id,
                UpdatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddMinutes(Session.MINUTES_UNTIL_EXPIRATION),
                User = user,
                Token = (Guid.NewGuid()).ToString()
            };
            return session;
        }

        public Session CreateSessionInDb(Session session)
        {
            using (var _db = new DatabaseContext())
            {
                _db.Sessions.Add(session);
                _db.SaveChanges();

                return session;
            }
        }

        public DatabaseContext CreateDataBaseContext()
        {
            return new DatabaseContext();
        }

        public bool isEqual(string[] arr1, string[] arr2)
        {
            if (arr1.Length != arr2.Length)
                return false;
            for (int i = 0; i < arr1.Length; i++)
            {
                if (arr1[i] != arr2[i])
                    return false;
            }
            return true;
        }
    }

}
