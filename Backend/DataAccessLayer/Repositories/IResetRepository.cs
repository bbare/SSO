using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    interface IResetRepository
    {
        void addToken(string userName, string token, DateTime expirationTime);
        void deleteToken(string token);
        DateTime getExpirationTime(string token);
        Guid getUserID(string token);
        string getToken(Guid userID);
        bool existingTokenGivenToken(string token);
        bool existingTokenGivenUsername(string token);
    }
}
