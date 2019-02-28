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
        public void addResetID(string userName, string resetIDToAdd, DateTime expirationTime)
        {
            //Query to get the userID of the provided username
            User userToAdd = ResetContext.Users.Find(userName);
            Guid userIDToAdd = userToAdd.Id;
            //Query to add all data to reset token table
            Reset tokenToAdd = new Reset
            {
                userID = userIDToAdd,
                resetID = resetIDToAdd,
                expirationTime = expirationTime
            };
            ResetContext.ResetIDs.Add(tokenToAdd);
        }

        //Function to delete the token from the DB
        public void deleteResetID(string resetID)
        {
            Reset tokenToRemove = ResetContext.ResetIDs.Find(resetID);
            ResetContext.ResetIDs.Remove(tokenToRemove);
        }

        //Function to get the expiration time of the token
        public DateTime getExpirationTime(string resetID)
        {
            Reset matchingToken = ResetContext.ResetIDs.Find(resetID);
            DateTime expirationTime = matchingToken.expirationTime;
            return expirationTime;
        }

        //Function to get the UserID associated with the reset token
        public Guid getUserID(string resetID)
        {
            Reset matchingToken = ResetContext.ResetIDs.Find(resetID);
            Guid userID = matchingToken.userID;
            return userID;
        }

        //Function to get the reset token given userID
        public string getResetID(Guid userID)
        {
            Reset resetIDToGet = ResetContext.ResetIDs.Find(userID);
            return resetIDToGet.resetID;
        }

        //Function to see if the token exists in the DB, given the token
        public bool existingResetIDGivenResetID(string resetID)
        {
            if(ResetContext.ResetIDs.Any(Reset => Reset.resetID == resetID))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //Function to see if the token exists in the DB, given the username
        public bool existingResetIDGivenUsername(string userName)
        {
            User userToCheckForResetToken = ResetContext.Users.Find(userName);
            Guid userIDToCheckForResetToken = userToCheckForResetToken.Id;
            if (ResetContext.ResetIDs.Any(Reset => Reset.userID == userIDToCheckForResetToken))
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
