using DataAccessLayer.Database;
using DataAccessLayer.Models;
using ServiceLayer.Services;
using System;
using System.Security.Cryptography;

namespace ManagerLayer
{
    public class AuthorizationManager : IAuthorizationManager
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
            return BitConverter.ToString(b).Replace("-", "").ToLower();
        }

        public Session CreateSession(DatabaseContext _db, User user)
        {
            Session session = new Session();
            session.Token = GenerateSessionToken();
            session.UserId = user.Id;

            var response = _sessionService.CreateSession(_db, session);

            return session;
        }

        public Session ValidateAndUpdateSession(DatabaseContext _db, string token)
        {
            Session response = _sessionService.GetSession(_db, token);
			//For the Compare method, a -1 is returned if the current Session expiration is shorter than the current time
			//A 0 is returned if they are equal
			//A 1 is returned if current Session is longer than the current time
			if (TimeSpan.Compare(response.ExpiresAt.TimeOfDay, DateTime.UtcNow.TimeOfDay) == -1 || TimeSpan.Compare(response.ExpiresAt.TimeOfDay, DateTime.UtcNow.TimeOfDay) == 0)
			{
				return _sessionService.UpdateSession(_db, response);
			}
			else
			{
				return null;
			}
		}

		public Session DeleteSession(DatabaseContext _db, string token)
        {
            return _sessionService.DeleteSession(_db, token);
        }
    }
}
