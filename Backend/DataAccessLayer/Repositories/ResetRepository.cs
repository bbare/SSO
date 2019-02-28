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
            Reset IDToAdd = new Reset
            {
                userID = userIDToAdd,
                resetID = resetIDToAdd,
                expirationTime = expirationTime
            };
            ResetContext.ResetIDs.Add(IDToAdd);
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

        //Function to update the password in the DB
        public void updatePassword(Guid userIDToChangePassword, string newPasswordHash)
        {
            using (ResetContext)
            {
                //Query the User table get the user that matches the UserID in the arguments
                var userToUpdate = ResetContext.Users.Find(userIDToChangePassword);
                if (userToUpdate != null)
                {
                    //Set that retrieved user's password hash to the new password hash
                    userToUpdate.PasswordHash = newPasswordHash;
                    ResetContext.SaveChanges();
                }
            }
        }

        //Function to get security questions from the DB
        public List<string> getSecurityQuestions(Guid userIDToGetQuestionsFrom)
        {
            List<string> listOfSecurityQuestions = new List<string>();
            var userToGetSecurityQuestionsFor = ResetContext.Users.Find(userIDToGetQuestionsFrom);
            var securityQ1 = userToGetSecurityQuestionsFor.SecurityQ1;
            var securityQ2 = userToGetSecurityQuestionsFor.SecurityQ2;
            var securityQ3 = userToGetSecurityQuestionsFor.SecurityQ3;
            listOfSecurityQuestions.Add(securityQ1);
            listOfSecurityQuestions.Add(securityQ2);
            listOfSecurityQuestions.Add(securityQ3);

            return listOfSecurityQuestions;
        }

        //Function to check security answers against the DB
        public bool checkSecurityAnswers(Guid userIDToGetQuestionsFrom, List<string> userSubmittedSecurityAnswers)
        {
            List<string> listOfSecurityAnswers = new List<string>();
            var userToGetSecurityQuestionsFor = ResetContext.Users.Find(userIDToGetQuestionsFrom);
            var securityA1 = userToGetSecurityQuestionsFor.SecurityQ1Answer;
            var securityA2 = userToGetSecurityQuestionsFor.SecurityQ2Answer;
            var securityA3 = userToGetSecurityQuestionsFor.SecurityQ3Answer;
            listOfSecurityAnswers.Add(securityA1);
            listOfSecurityAnswers.Add(securityA2);
            listOfSecurityAnswers.Add(securityA3);

            for(int i = 0; i < listOfSecurityAnswers.Count; i++)
            {
                if(listOfSecurityAnswers[i] != userSubmittedSecurityAnswers[i])
                {
                    return false;
                }
            }
            return true;
        }
    }
}
