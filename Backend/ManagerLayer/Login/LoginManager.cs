using DataAccessLayer.Models;
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
                if(_passwordService.VerifyPassword(password, user.PasswordHash, user.PasswordSalt))
                {
                    return true;
                }
                else
                {
                    user.IncorrectPasswordCount = user.IncorrectPasswordCount++;
                    if(user.IncorrectPasswordCount == 3)
                    {
                        user.Disabled = true;
                    }
                    return false;
                }
            }
        }

        public Session LoginAuthorized()
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
                return response;
            }
        }
    }
}
