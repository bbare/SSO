using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Models;
using DataAccessLayer.Database;

namespace DataAccessLayer.Repositories
{
    class ResetRepository
    {
        public string addToken(string userName, string token, DateTime expirationTime)
        {
            //Query to get the userID of the provided username
            //Query to add all data to reset token table
        }

        public string deleteToken(string token)
        {

        }

        public DateTime getExpirationTime()
        {

        }
        public Guid getUserID()
        {

        }

        public string getToken()
        {

        }

        public bool existingToken(string token)
        {

        }
    }
}
