using DataAccessLayer.Models;
using DataAccessLayer.Repositories;
using ServiceLayer.Services;
using DataAccessLayer.Database;
using System.Net.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagerLayer.Login
{
    public class LoginManager
    {
        IUserService _userService = new UserService();
        IPasswordService _passwordService = new PasswordService();
        ITokenService _tokenService = new TokenService();
        ISessionService _sessionService = new SessionService();
        UserRepository userRepo = new UserRepository();
        private User user;

        public LoginManager()
        {

        }

        public bool LoginCheckUserExists(string email)
        {
            using (var _db = new DatabaseContext())
            {
                user = _userService.GetUser(_db, email);
                if (user != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
                
            }
        }

        public bool LoginCheckUserDisabled()
        {
            using (var _db = new DatabaseContext())
            {
                if (user.Disabled)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool LoginCheckPassword(string password)
        {
            using (var _db = new DatabaseContext())
            {
                string hashedPassword = _passwordService.HashPassword(password, user.PasswordSalt);
                if (userRepo.ValidatePassword(user, hashedPassword))
                {
                    return true;
                }
                else
                {
                    user.IncorrectPasswordCount = user.IncorrectPasswordCount++;
                    _db.SaveChanges();
                    if(user.IncorrectPasswordCount == 3)
                    {
                        user.Disabled = true;
                        _db.SaveChanges();
                    }
                    return false;
                }
            }
        }

        public string LoginAuthorized()
        {
            using (var _db = new DatabaseContext())
            {
                string generateToken = _tokenService.GenerateToken();
                Session session = new Session
                {
                    Token = generateToken,
                    User = user
                };

                var response = _sessionService.CreateSession(_db, session);
                _db.SaveChanges();
                return session.Token;
            }
        }
    }
}