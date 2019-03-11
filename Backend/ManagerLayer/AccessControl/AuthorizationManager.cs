using DataAccessLayer.Database;
using DataAccessLayer.Models;
using ServiceLayer.Services;
using System;
using System.Data.Entity.Validation;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagerLayer.AccessControl
{
    class AuthorizationManager
    {
        private ISessionService _sessionService;
        private IUserService _userService;
        private ITokenService _tokenService;

        private DatabaseContext CreateDbContext()
        {
            return new DatabaseContext();
        }

        public AuthorizationManager()
        {
             _sessionService = new SessionService();
        }

        public string CreateSession(User user)
        {
            _userService = new UserService();
            _tokenService = new TokenService();
            using (var _db = CreateDbContext())
            {
                var userResponse = _userService.GetUser(_db, user.Email);
                if(userResponse == null)
                {
                    return null;
                }
                Session session = new Session();
                session.Token = _tokenService.GenerateToken();
                session.User = user;
                session.UserId = user.Id;

                var response = _sessionService.CreateSession(_db, session);
                try
                {
                    _db.SaveChanges();
                    return response.Token;
                }
                catch (DbEntityValidationException)
                {
                    //catch error
                    // detach session attempted to be created from the db context - rollback
                    _db.Entry(response).State = System.Data.Entity.EntityState.Detached;
                }
            }
            return null;
        }

        public string ValidateAndUpdateSession(string token, Guid userId)
        {
            using (var _db = CreateDbContext())
            {
                Session response = _sessionService.ValidateSession(_db, token, userId);

                if(response != null)
                {
                    response = _sessionService.UpdateSession(_db, response);
                }
                else
                {
                    return null;
                }

                try
                {
                    _db.SaveChanges();
                    return response.Token;
                }
                catch (DbEntityValidationException)
                {
                    //catch error
                    // detach session attempted to be created from the db context - rollback
                    _db.Entry(response).State = System.Data.Entity.EntityState.Detached;
                }
            }
            return null;
        }

        public int DeleteSession(string token, Guid userId)
        {
            using (var _db = new DatabaseContext())
            {
                Session response = _sessionService.DeleteSession(_db, token, userId);

                return _db.SaveChanges();
            }
        }
    }
}
