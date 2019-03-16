using DataAccessLayer.Database;
using DataAccessLayer.Models;
using ServiceLayer.Services;
using System;
using System.Security.Cryptography;

namespace ManagerLayer
{
    public class AuthorizationManager
    {
        private ISessionService _sessionService;
        private ITokenService _tokenService;

        public AuthorizationManager()
        {
            _sessionService = new SessionService();
            _tokenService = new TokenService();
        }

        public string GenerateSessionToken()
        {
            return _tokenService.GenerateToken();
        }

        public Session CreateSession(DatabaseContext _db, User user)
        {
            Session session = new Session();
            session.Token = GenerateSessionToken();
            session.User = user;
            session.UserId = user.Id;

            var response = _sessionService.CreateSession(_db, session);

            return session;
        }

        public Session ValidateAndUpdateSession(DatabaseContext _db, string token, Guid userId)
        {
            Session response = _sessionService.ValidateSession(_db, token, userId);

            if(response != null)
            {
                return _sessionService.UpdateSession(_db, response);
            }
            else
            {
                return null;
            }
        }

        public void DeleteSession(DatabaseContext _db, string token, Guid userId)
        {
            _sessionService.DeleteSession(_db, token, userId);
        }
    }
}
