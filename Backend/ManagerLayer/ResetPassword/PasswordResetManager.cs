using DataAccessLayer.Database;
using ServiceLayer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Models;
using MimeKit;

namespace ManagerLayer.ResetPassword
{
    public class PasswordResetManager
    {
        //Variable for how long the token is supposed to be live
        private const double TimeToExpire = 5;

        private IResetService _resetService;
        private IUserService _userService;
        private IPasswordService _passwordService;
        private IEmailService _emailService;

        public PasswordResetManager()
        {
            _resetService = new ResetService();
            _userService = new UserService();
            _emailService = new EmailService();
        }

        private DatabaseContext CreateDbContext()
        {
            return new DatabaseContext();
        }

        public PasswordReset CreatePasswordReset(string email)
        {

            #region ResetID Generation
            RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider();
            //List used to hold the two random numbers that will be generated
            List<string> listOfInts = new List<string>();
            //While loop to continue generating the numbers until satisfies length requirement
            while (listOfInts.Count < 2)
            {
                byte[] randomNumber = new byte[8];
                rngCsp.GetBytes(randomNumber);
                int result = BitConverter.ToInt32(randomNumber, 0);
                //If the number that was generated is negative, make it positive
                if (result < 0)
                {
                    result = result * -1;
                }
                //Convert the number to a string
                string resultToString = result.ToString();
                //Add the number to the list of random numbers generated
                listOfInts.Add(resultToString);
            }
            //Concatenate the two numbers generated to have a resetID that is minimum of 18 characters long
            string generatedResetID = listOfInts[0] + listOfInts[1];

            #endregion 

            //Expiration time for the resetID
            DateTime newExpirationTime = DateTime.Now.AddMinutes(TimeToExpire);

            using (var _db = CreateDbContext())
            {
                User associatedUser = _userService.GetUser(_db, email);
                PasswordReset passwordReset = new PasswordReset
                {
                    resetID = generatedResetID,
                    userID = associatedUser.Id,
                    expirationTime = newExpirationTime,
                    resetCount = 0,
                    disabled = false
                };

                associatedUser.numOfResetAttempts = associatedUser.numOfResetAttempts + 1;
                _userService.UpdateUser(_db, associatedUser);
                _db.SaveChanges();

                return _resetService.CreatePasswordReset(_db, passwordReset);
            }
        }

        public PasswordReset DeletePasswordReset(string resetID)
        {
            using (var _db = CreateDbContext())
            {
                return _resetService.DeletePasswordReset(_db, resetID);
            }
        }

        public PasswordReset GetPasswordReset(string resetID)
        {
            using (var _db = CreateDbContext())
            {
                return _resetService.GetPasswordReset(_db, resetID);
            }
        }

        public PasswordReset UpdatePasswordReset(PasswordReset updatedPasswordReset)
        {
            using (var _db = CreateDbContext())
            {
                return _resetService.UpdatePasswordReset(_db, updatedPasswordReset);
            }
        }

        public DateTime GetPasswordResetExpiration(string resetID)
        {
            var resetIDRetrieved = GetPasswordReset(resetID);
            return resetIDRetrieved.expirationTime;
        }

        public bool ExistingResetID(string resetID)
        {
            using (var _db = CreateDbContext())
            {
                return _resetService.ExistingReset(_db, resetID);
            }
        }

        public int GetAttemptsPerID(string resetID)
        {
            var retrievedResetID = GetPasswordReset(resetID);
            return retrievedResetID.resetCount;
        }

        public bool GetResetIDStatus(string resetID)
        {
            var retrievedResetID = GetPasswordReset(resetID);
            return retrievedResetID.disabled;
        }

        public bool checkResetIDValid(string resetID)
        {
            //See if ResetID exists 
            if (ExistingResetID(resetID))
            {
                if (!GetResetIDStatus(resetID))
                {
                    if (GetAttemptsPerID(resetID) < 4)
                    {
                        if (GetPasswordResetExpiration(resetID) > DateTime.Now)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public string CreateResetURL(string resetID)
        {
            string resetControllerURL = "kfcsso.com/api/reset/resetpassword/?id=";
            string resetURL = resetControllerURL + resetID;
            return resetURL;
        }

        public void LockResetID(string resetID)
        {
            var retrievedPasswordReset = GetPasswordReset(resetID);
            retrievedPasswordReset.disabled = true;
            UpdatePasswordReset(retrievedPasswordReset);
        }


        //Needs to getUser before being able to call updatePassword
        public bool UpdatePassword(User userToUpdate, string newPasswordHash)
        {
            var userID = userToUpdate.Id;

            using (var _db = CreateDbContext())
            {
                //Query the User table get the user that matches the UserID in the arguments
                var storedHash = _db.Users.Find(userID).PasswordHash;

                //Check to see if the new password is the same as the old password
                if (storedHash == newPasswordHash)
                {
                    return false;
                }
                else //If the new password is different, then update the password
                {
                    //Set that retrieved user's password hash to the new password hash
                    storedHash = newPasswordHash;
                    userToUpdate.numOfResetAttempts = 0;
                    _userService.UpdateUser(_db, userToUpdate);
                    _db.SaveChanges();
                    return true;
                }
            }
        }

        //Function to get security questions from the DB
        public List<string> GetSecurityQuestions(string resetID)
        {
            List<string> listOfSecurityQuestions = new List<string>();
            var retrievedPasswordReset = GetPasswordReset(resetID);
            var userID = retrievedPasswordReset.userID;

            using (var _db = CreateDbContext())
            {
                var securityQ1 = _db.Users.Find(userID).SecurityQ1;
                var securityQ2 = _db.Users.Find(userID).SecurityQ2;
                var securityQ3 = _db.Users.Find(userID).SecurityQ3;
                listOfSecurityQuestions.Add(securityQ1);
                listOfSecurityQuestions.Add(securityQ2);
                listOfSecurityQuestions.Add(securityQ3);
                return listOfSecurityQuestions;
            }
        }

        //Function to check security answers against the DB
        public bool CheckSecurityAnswers(User user, List<string> userSubmittedSecurityAnswers)
        {
            using (var _db = CreateDbContext())
            {
                List<string> listOfSecurityAnswers = new List<string>();

                var securityA1 = _db.Users.Find(user.Id).SecurityQ1Answer;
                var securityA2 = _db.Users.Find(user.Id).SecurityQ2Answer;
                var securityA3 = _db.Users.Find(user.Id).SecurityQ3Answer;
                listOfSecurityAnswers.Add(securityA1);
                listOfSecurityAnswers.Add(securityA2);
                listOfSecurityAnswers.Add(securityA3);
                for (int i = 0; i < listOfSecurityAnswers.Count; i++)
                {
                    //If the answers provided don't match the answers in the DB, the number of attempts to reset the password with that resetID is incremented
                    if (listOfSecurityAnswers[i] != userSubmittedSecurityAnswers[i])
                    {
                        var resetIDToCount = _db.ResetIDs.Find(user.Id);
                        if (resetIDToCount != null)
                        {
                            int count = resetIDToCount.resetCount;
                            count++;
                            resetIDToCount.resetCount = count;
                            _db.SaveChanges();
                        }
                        return false;
                    }
                }
                return true;
            }
        }

        #region Email Functions
        //Function to create the email is user exists 
        public void SendResetEmailUserExists(string receiverEmail, string resetURL)
        {
            string resetPasswordSubjectString = "KFC SSO Reset Password";
            string userFullName = receiverEmail;
            string template = "Hi, \r\n" +
                                             "You recently requested to reset your password for your KFC account, click the link below to reset it.\r\n" +
                                             "The URL is only valid for the next 5 minutes\r\n {0}" +
                                             "If you did not request to reset your password, please contact us by responding to this email.\r\n\r\n" +
                                             "Thanks, KFC Team";
            string data = "resetURL";
            string resetPasswordBodyString = string.Format(template, data);

            //Create the message that will be sent
            MimeMessage emailToSend = _emailService.createEmailPlainBody(userFullName, receiverEmail, resetPasswordSubjectString, resetPasswordBodyString);
            //Send the email with the message
            _emailService.sendEmail(emailToSend);

        }

        //Function to create the email is user doesn't exist
        public void SendResetEmailUserDoesNotExist(string receiverEmail)
        {
            string resetPasswordSubjectString = "KFC SSO Reset Password";
            string userFullName = "Unknown";
            string resetPasswordUserDoesNotExistEmailBody = "Hello, \r\n" +
                              "You (or someone else) entered this email address when trying to reset the password of a KFC account.\r\n" +
                              "However, this email address is not on our database of registered users and therefore the attempt to reset the password has failed.\r\n" +
                              "If you have a KFC account and were expecting this email, please try again using the email address you gave when opening your account." +
                              "If you do not have a KFC account, please ignore this email.\r\n" +
                              "For more information about KFC, please visit www.kfc.com/faq \r\n\r\n" +
                              "Best Regards, KFC Team";

            //Create the message that will be sent
            MimeMessage emailToSend = _emailService.createEmailPlainBody(userFullName, receiverEmail, resetPasswordSubjectString, resetPasswordUserDoesNotExistEmailBody);
            //Send the email with the message
            _emailService.sendEmail(emailToSend);
        }

        //Function to create the email if the password was changed
        public void SendPasswordChange(string receiverEmail)
        {
            //Need SQL Query to get info about user from DB
            string resetPasswordSubjectString = "KFC SSO Reset Password";
            string userFullName = receiverEmail;
            string template = "Hi {0}, \r\n" +
                                             "You have changed your password on KFC SSO.\r\n" +
                                             "If you did not change your password, please contact us by responding to this email.\r\n\r\n" +
                                             "Thanks, KFC Team";
            string data = "userFirstName";
            //Fill the text with information
            string resetPasswordBodyString = string.Format(template, data);
            //Create the email service object
            EmailService es = new EmailService();
            //Create the message that will be sent
            MimeMessage emailToSend = _emailService.createEmailPlainBody(userFullName, receiverEmail, resetPasswordSubjectString, resetPasswordBodyString);
            //Send the email with the message
            _emailService.sendEmail(emailToSend);
        }
        #endregion

    }
}
