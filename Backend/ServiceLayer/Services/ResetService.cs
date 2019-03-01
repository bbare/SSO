using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Database;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories;
using MimeKit;
using MailKit;
using MailKit.Net.Smtp;


namespace ServiceLayer.Services
{
    public class ResetService
    {

        #region Constants for the reset service
        //Variable for how long the token is supposed to be live
        private const double TimeToExpire = 5;
        //Constant for the password reset email subject line
        private const string resetPasswordSubjectString = "KFC SSO Password Reset";
        //URL to append the reset ID to. Sent to the user in the email body.
        private const string resetControllerURL = "kfcsso.com/api/reset/resetpassword/?id=";

        #endregion

        private ResetRepository _resetRepo;
        public ResetService()
        {
            _resetRepo = new ResetRepository(new DatabaseContext());
        }

        //Function to create token in the system
        public string createResetID()
        {
            
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
            string resetID = listOfInts[0] + listOfInts[1];

            return resetID;
        }

        public string createResetURL(string resetID)
        {
            string resetURL = resetControllerURL + resetID;
            return resetURL;
        }

        //Function to add the token to the database
        public void addResetIDToDB(string userName, string resetID)
        {
            //Gets current time, and adds the time to expire constant
            DateTime expirationTime = DateTime.Now.AddMinutes(TimeToExpire);
            _resetRepo.addResetID(userName, resetID, expirationTime);
        }

        //Read the token given from the URL
        public bool isResetIDIsExpired(string resetID)
        {
            //Check to see if the ResetID exists in the DB first
            if (_resetRepo.existingResetIDGivenResetID(resetID))
            {
                //Get the time when the resetID is to expire
                DateTime when = _resetRepo.getExpirationTime(resetID);
                //Compare the expiration date to the time 
                if (when > DateTime.Now)
                {
                    return true; //True if the expiration date is after the time now
                }
                //Case where token has expired, delete token
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        //Function to check if user already has a resetID
        public bool checkUserHasResetID(string email)
        {
            return _resetRepo.existingResetIDGivenUsername(email);
        }

        //Function to get the resetID
        public string getResetID(string email)
        {
            return _resetRepo.getResetID(email);
        }

        //Function to delete the ResetID from DB
        public void deleteResetIDFromDB(string resetID)
        {
            _resetRepo.deleteResetID(resetID);
        }

        //Function to get the security questions of the user associated with the resetID
        public List<string> getSecurityQuestionsFromDB(string resetID)
        {
            Guid userIDToGetQuestionsFor = _resetRepo.getUserID(resetID);
            List<string> securityQuestions = _resetRepo.getSecurityQuestions(userIDToGetQuestionsFor);
            return securityQuestions;
        }

        //Function to check the security answers of the user associated with the resetID
        public bool checkSecurityAnswers(string resetID, List<string> userSubmittedAnswers)
        {
            //Get the UserID of the user whose security answers we're checking against
            Guid userIDToCheckAnswersFor = _resetRepo.getUserID(resetID);
            //Check the security answers
            return _resetRepo.checkSecurityAnswers(userIDToCheckAnswersFor, userSubmittedAnswers);
        }

        //Function to lock the resetID for 24 hours if failed to answer security questions 3 times
        public void lockResetID(string resetID)
        {
            _resetRepo.lockOut(resetID);
        }

        //Function to check if the resetID is locked out
        public bool isLockedOut(string resetID)
        {
            return _resetRepo.checkLockOut(resetID);
        }

        //Function to check how many times a reset has been attempted with the resetID
        public int getResetCount(string resetID)
        {
            return _resetRepo.getAttemptsPerResetID(resetID);
        }

        //Function to see how many resetIDs are associated with the given email
        public int getResetIDCount(string email)
        {
            return _resetRepo.getResetIDCountPerEmail(email);
        }
        
        #region Email Functions
        //Function to create the email is user exists 
        public void sendResetEmailUserExists(string receiverEmail, string resetURL)
        {
            //Need SQL Query to get info about user from DB
            string userFirstName = "";
            string userlastName = "";
            string userFullName = userFirstName + " " + userlastName;
            string template = "Hi {0}, \r\n" +
                                             "You recently requested to reset your password for your KFC account, click the link below to reset it.\r\n" +
                                             "The URL is only valid for the next 5 minutes\r\n {1}" +
                                             "If you did not request to reset your password, please contact us by responding to this email.\r\n\r\n" +
                                             "Thanks, KFC Team";
            string data = "userFirstName, resetURL";
            string resetPasswordBodyString = string.Format(template, data);
            //Create the email service object
            EmailService es = new EmailService();
            //Create the message that will be sent
            MimeMessage emailToSend = es.createEmailPlainBody(userFullName, receiverEmail, resetPasswordSubjectString, resetPasswordBodyString);
            //Send the email with the message
            es.sendEmail(emailToSend);
            
        }

        //Function to create the email is user doesn't exist
        public void sendResetEmailUserDoesNotExist(string receiverEmail)
        {
            string userFullName = "Unknown";
            string resetPasswordUserDoesNotExistEmailBody = "Hello, \r\n" +
                              "You (or someone else) entered this email address when trying to reset the password of a KFC account.\r\n" +
                              "However, this email address is not on our database of registered users and therefore the attempt to reset the password has failed.\r\n" +
                              "If you have a KFC account and were expecting this email, please try again using the email address you gave when opening your account." +
                              "If you do not have a KFC account, please ignore this email.\r\n" +
                              "For more information about KFC, please visit www.kfc.com/faq \r\n\r\n" +
                              "Best Regards, KFC Team";
            //Create the email service object
            EmailService es = new EmailService();
            //Create the message that will be sent
            MimeMessage emailToSend = es.createEmailPlainBody(userFullName, receiverEmail, resetPasswordSubjectString, resetPasswordUserDoesNotExistEmailBody);
            //Send the email with the message
            es.sendEmail(emailToSend);
        }

        //Function to create the email if the password was changed
        public void sendPasswordChange(string receiverEmail)
        {
            //Need SQL Query to get info about user from DB
            string userFirstName = "";
            string userlastName = "";
            string userFullName = userFirstName + " " + userlastName;
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
            MimeMessage emailToSend = es.createEmailPlainBody(userFullName, receiverEmail, resetPasswordSubjectString, resetPasswordBodyString);
            //Send the email with the message
            es.sendEmail(emailToSend);
        }
        #endregion
    }
}
