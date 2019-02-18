using System;
using System.Security.Cryptography;
using DataAccessLayer.Database;
using DataAccessLayer.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessLayer.Repositories;

namespace ServiceLayer.Services
{
    public class SessionService : ISessionService
    {
        private SessionReposity _SessionRepo;

        public SessionService()
        {
            _SessionRepo = new SessionReposity();
        }

        public string GenerateSession()
        {
            RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
            Byte[] b = new byte[64 /2];
            provider.GetBytes(b);
            string hex = BitConverter.ToString(b).Replace("-","");
            return hex;
        }

        public bool ValidateSession(User user)
        {
            return _SessionRepo.ValidateSession(user.Id);
        }
    }
}
