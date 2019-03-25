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

            User u = new User
            {
                Id = Guid.NewGuid(),
                Email = Guid.NewGuid() + "@" + Guid.NewGuid() + ".com",
                DateOfBirth = DateTime.UtcNow,
                City = "Los Angeles",
                State = "California",
                Country = "United States",
                SecurityQ1 = "MySecurityQ1",
                SecurityQ1Answer = "MySecurityAnswerQ1",
                SecurityQ2 = "MySecurityQ2",
                SecurityQ2Answer = "MySecurityAnswerQ2",
                SecurityQ3 = "MySecurityQ3",
                SecurityQ3Answer = "MySecurityAnswerQ3",
                PasswordHash = (Guid.NewGuid()).ToString(),
                PasswordSalt = GetRandomness()
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
                SecurityQ1 = "MySecurityQ1",
                SecurityQ1Answer = "MySecurityAnswerQ1",
                SecurityQ2 = "MySecurityQ2",
                SecurityQ2Answer = "MySecurityAnswerQ2",
                SecurityQ3 = "MySecurityQ3",
                SecurityQ3Answer = "MySecurityAnswerQ3",
                PasswordHash = (Guid.NewGuid()).ToString(),
                PasswordSalt = GetRandomness()
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
        
        public PasswordReset CreatePasswordResetInDB()
        {
            PasswordReset pr = new PasswordReset
            {
                PasswordResetID = new Guid(),
                ResetToken = "",
                UserID = new Guid(),
                ExpirationTime = DateTime.Now.AddMinutes(5),
                ResetCount = 0,
                Disabled = false
            };
            return CreatePasswordResetInDB(pr);
        }

        public PasswordReset CreatePasswordResetInDB(PasswordReset resetToken)
        {
            using (var _db = new DatabaseContext())
            {
                _db.Entry(resetToken).State = System.Data.Entity.EntityState.Added;
                _db.SaveChanges();
                return resetToken;
            }
        }

        public PasswordReset CreatePasswordResetObject()
        {
            PasswordReset pr = new PasswordReset
            {
                PasswordResetID = new Guid(),
                ResetToken = "",
                UserID = new Guid(),
                ExpirationTime = DateTime.Now.AddMinutes(5),
                ResetCount = 0,
                Disabled = false
            };
            return pr;
        }

        public Application CreateApplicationInDb()
        {

            Application app = new Application
            {
                Id = Guid.NewGuid(),
                Title = "KFC App",
                LaunchUrl = "https://kfc.com",
                Email = "kfc@email.com",
                UserDeletionUrl = "https://kfc.com/delete",
                LogoUrl = "https://kfc.com/logo.png",
                Description = "A KFC app",
                SharedSecretKey = Guid.NewGuid().ToString("N")
            };

            return CreateApplicationInDb(app);
        }

        public Application CreateApplicationInDb(Application app)
        {
            using (var _db = new DatabaseContext())
            {
                _db.Entry(app).State = System.Data.Entity.EntityState.Added;
                _db.SaveChanges();

                return app;
            }
        }

        public Application CreateApplicationObject()
        {
            Application app = new Application
            {
                Id = Guid.NewGuid(),
                Title = "KFC App",
                LaunchUrl = "https://kfc.com",
                Email = "kfc@email.com",
                UserDeletionUrl = "https://kfc.com/delete",
                LogoUrl = "https://kfc.com/logo.png",
                Description = "A KFC app",
                SharedSecretKey = Guid.NewGuid().ToString("N")
            };
            return app;
        }

        public ApiKey CreateApiKeyInDb()
        {
            Application app = CreateApplicationObject();
            ApiKey apiKey = new ApiKey
            {
                Id = Guid.NewGuid(),
                Key = Guid.NewGuid().ToString("N"),
                ApplicationId = app.Id,
                IsUsed = false
            };

            return CreateApiKeyInDb(apiKey);
        }

        public ApiKey CreateApiKeyInDb(ApiKey apiKey)
        {
            using (var _db = new DatabaseContext())
            {
                _db.Entry(apiKey).State = System.Data.Entity.EntityState.Added;
                _db.SaveChanges();

                return apiKey;
            }
        }

        public ApiKey CreateApiKeyObject()
        {
            Application app = CreateApplicationObject();
            ApiKey apiKey = new ApiKey
            {
                Id = Guid.NewGuid(),
                Key = Guid.NewGuid().ToString("N"),
                ApplicationId = app.Id,
                IsUsed = false
            };
            return apiKey;
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
