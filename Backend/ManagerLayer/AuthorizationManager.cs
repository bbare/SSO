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

        public AuthorizationManager()
        {
             _sessionService = new SessionService();
        }

        public string GenerateSessionToken()
        {
            RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
            Byte[] b = new byte[64 / 2];
            provider.GetBytes(b);
            string hex = BitConverter.ToString(b).Replace("-", "");
            return hex;
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
