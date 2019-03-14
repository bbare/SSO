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
        ISessionService _sessionService = new SessionService();
        UserRepository userRepo = new UserRepository();

        private User user;
        private ITokenService _tokenService;

        public LoginManager()
        {
            _tokenService = new TokenService();
        }

        public bool LoginCheckUserExists(LoginRequest request)
        {
            using (var _db = new DatabaseContext())
            {
                user = _userService.GetUser(_db, request.email);
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

        public bool LoginCheckUserDisabled(LoginRequest request)
        {
            using (var _db = new DatabaseContext())
            {
                user = _userService.GetUser(_db, request.email);
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

        public bool LoginCheckPassword(LoginRequest request)
        {
            using (var _db = new DatabaseContext())
            {
                user = _userService.GetUser(_db, request.email);
                string hashedPassword = _passwordService.HashPassword(request.password, user.PasswordSalt);
                if (userRepo.ValidatePassword(user, hashedPassword))
                {
                    user.IncorrectPasswordCount = 0;
                    _userService.UpdateUser(_db, user);
                    _db.SaveChanges();
                    return true;
                }
                else
                {
                    user.IncorrectPasswordCount = ++user.IncorrectPasswordCount;
                    _userService.UpdateUser(_db, user);
                    _db.SaveChanges();
                    if (user.IncorrectPasswordCount == 3)
                    {
                        user.Disabled = true;
                        _userService.UpdateUser(_db, user);
                        _db.SaveChanges();
                    }
                    return false;
                }
            }
        }

        public string LoginAuthorized(LoginRequest request)
        {
            _tokenService = new TokenService();
            using (var _db = new DatabaseContext())
            {
                user = _userService.GetUser(_db, request.email);
                string generateToken = _tokenService.GenerateToken();
                Session session = new Session
                {
                    Token = generateToken,
                    UserId = user.Id
                };

                var response = _sessionService.CreateSession(_db, session);
                _db.SaveChanges();
                return session.Token;
            }
        }
    }
}