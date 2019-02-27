using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Models;
using DataAccessLayer.Database;

namespace DataAccessLayer.Repositories
{
    public class ResetRepository: IResetRepository
    {
        private DatabaseContext ResetContext;
        public ResetRepository(DatabaseContext _db)
        {
            this.ResetContext = _db;
        }

        //Function to add the token to the DB
        public void addToken(string userName, string token, DateTime expirationTime)
        {
            //Query to get the userID of the provided username
            User userToAdd = ResetContext.Users.Find(userName);
            Guid userIDToAdd = userToAdd.Id;
            //Query to add all data to reset token table
            ResetToken tokenToAdd = new ResetToken
            {
                userID = userIDToAdd,
                resetTokenString = token,
                expirationTime = expirationTime
            };
            ResetContext.ResetTokens.Add(tokenToAdd);
        }

        //Function to delete the token from the DB
        public void deleteToken(string token)
        {
            ResetToken tokenToRemove = ResetContext.ResetTokens.Find(token);
            ResetContext.ResetTokens.Remove(tokenToRemove);
        }

        //Function to get the expiration time of the token
        public DateTime getExpirationTime(string token)
        {
            ResetToken matchingToken = ResetContext.ResetTokens.Find(token);
            DateTime expirationTime = matchingToken.expirationTime;
            return expirationTime;
        }

        //Function to get the UserID associated with the reset token
        public Guid getUserID(string token)
        {
            ResetToken matchingToken = ResetContext.ResetTokens.Find(token);
            Guid userID = matchingToken.userID;
            return userID;
        }

        //Function to get the reset token given userID
        public string getToken(Guid userID)
        {
            ResetToken tokenToGet = ResetContext.ResetTokens.Find(userID);
            return tokenToGet.resetTokenString;
        }

        //Function to see if the token exists in the DB, given the token
        public bool existingTokenGivenToken(string token)
        {
            if(ResetContext.ResetTokens.Any(ResetToken => ResetToken.resetTokenString == token))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //Function to see if the token exists in the DB, given the username
        public bool existingTokenGivenUsername(string userName)
        {
            User userToCheckForResetToken = ResetContext.Users.Find(userName);
            Guid userIDToCheckForResetToken = userToCheckForResetToken.Id;
            if (ResetContext.ResetTokens.Any(ResetToken => ResetToken.userID == userIDToCheckForResetToken))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
