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
        public void AddResetID(string userName, string resetIDToAdd, DateTime expirationTime)
        {
            //Query to get the userID of the provided username
            User userToAdd = ResetContext.Users.Find(userName);
            Guid userIDToAdd = userToAdd.Id;
            //Query to add all data to reset token table
            Reset IDToAdd = new Reset
            {
                userID = userIDToAdd,
                resetID = resetIDToAdd,
                expirationTime = expirationTime,
                resetCount = 0,
                lockedOut = false
            };
            ResetContext.ResetIDs.Add(IDToAdd);
        }

        //Function to delete the token from the DB
        public void DeleteResetID(string resetID)
        {
            Reset tokenToRemove = ResetContext.ResetIDs.Find(resetID);
            ResetContext.ResetIDs.Remove(tokenToRemove);
        }

        //Function to get the expiration time of the token
        public DateTime GetExpirationTime(string resetID)
        {
            Reset matchingToken = ResetContext.ResetIDs.Find(resetID);
            DateTime expirationTime = matchingToken.expirationTime;
            return expirationTime;
        }

        //Function to get the UserID associated with the reset token
        public Guid GetUserID(string resetID)
        {
            Reset matchingToken = ResetContext.ResetIDs.Find(resetID);
            Guid userID = matchingToken.userID;
            return userID;
        }

        //Function to get the reset token given userID
        public string GetResetID(Guid userID)
        {
            Reset resetIDToGet = ResetContext.ResetIDs.Find(userID);
            return resetIDToGet.resetID;
        }

        //Function to get the resetID given email
        public string GetResetID(string email)
        {
            User userToCheckForResetToken = ResetContext.Users.Find(email);
            Guid userID = userToCheckForResetToken.Id;
            var reset = ResetContext.ResetIDs.Find(userID);
            return reset.resetID;
        }

        //Function to see if the token exists in the DB, given the token
        public bool ExistingResetIDGivenResetID(string resetID)
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
        public bool ExistingResetIDGivenEmail(string userName)
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
        public bool UpdatePassword(Guid userIDToChangePassword, string newPasswordHash)
        {
            using (ResetContext)
            {
                //Query the User table get the user that matches the UserID in the arguments
                var userToUpdate = ResetContext.Users.Find(userIDToChangePassword);
                if (userToUpdate != null)
                {
                    //Check to see if the new password is the same as the old password
                    if(userToUpdate.PasswordHash == newPasswordHash)
                    {
                        return false;
                    }
                    else //If the new password is different, then update the password
                    {
                        //Set that retrieved user's password hash to the new password hash
                        userToUpdate.PasswordHash = newPasswordHash;
                        ResetContext.SaveChanges();
                        return true;
                    }
                    
                }
            }
            //Default case is password isn't updated
            return false;
        }

        //Function to get security questions from the DB
        public List<string> GetSecurityQuestions(Guid userIDToGetQuestionsFrom)
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
        public bool CheckSecurityAnswers(Guid userIDToGetQuestionsFrom, List<string> userSubmittedSecurityAnswers)
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
                //If the answers provided don't match the answers in the DB, the number of attempts to reset the password with that resetID is incremented
                if(listOfSecurityAnswers[i] != userSubmittedSecurityAnswers[i])
                {
                    var resetIDToCount = ResetContext.ResetIDs.Find(userIDToGetQuestionsFrom);
                    if(resetIDToCount != null)
                    {
                        int count = resetIDToCount.resetCount;
                        count++;
                        resetIDToCount.resetCount = count;
                        ResetContext.SaveChanges();
                    }
                    return false;
                }
            }
            return true;
        }

        //Function to toggle lockout
        public void LockOut(string resetID)
        {
            using (ResetContext)
            {
                var resetIDToLock = ResetContext.ResetIDs.Find(resetID);
                if(resetIDToLock != null)
                {
                    resetIDToLock.lockedOut = true;
                    resetIDToLock.lockoutTime = DateTime.Now.AddHours(24);
                    ResetContext.SaveChanges();
                }
            }
        }

        //Function to check if locked out, returns true if still locked out, false if not locked out
        public bool CheckLockOut(string resetID)
        {
            using (ResetContext)
            {
                var resetIDToCheckForLockOut = ResetContext.ResetIDs.Find(resetID);
                if(resetIDToCheckForLockOut != null)
                {
                    return resetIDToCheckForLockOut.lockoutTime > DateTime.Now;
                }
            }
            return true;
        }

        //Function to get the amount of times the resetID has been used to attempt a password reset
        public int GetAttemptsPerResetID(string resetID)
        {
            using (ResetContext)
            {
                var resetIDToCount = ResetContext.ResetIDs.Find(resetID);
                if(resetIDToCount != null)
                {
                    return resetIDToCount.resetCount;
                }
            }
            return 3;
        }

        public int GetResetIDCountPerEmail(string email)
        {
            User userToCheckForResetID = ResetContext.Users.Find(email);
            Guid userIDToCheckForResetID = userToCheckForResetID.Id;
            var count = ResetContext.ResetIDs
                .Where(reset => reset.userID == userIDToCheckForResetID)
                .Count();
            return count;
        }

    }
}
