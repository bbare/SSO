using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Database;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories;


namespace ServiceLayer.Services
{
    public class ResetService
    {
        //Variable for how long the token is supposed to be live
        private const double TimeToExpire = 5;
        //Constant for the password reset email subject line
        private const string resetPasswordSubjectString = "KFC SSO Password Reset";

        private const string resetControllerURL = "kfcsso.com/api/reset/resetpassword/?id=";

        private ResetRepository _resetRepo;
        public ResetService()
        {
            _resetRepo = new ResetRepository(new DatabaseContext());
        }

        //Function to create token in the system
        public string createResetID()
        {
            RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider();
            List<string> listOfInts = new List<string>();
            while (listOfInts.Count < 2)
            {
                byte[] randomNumber = new byte[8];
                rngCsp.GetBytes(randomNumber);
                int result = BitConverter.ToInt32(randomNumber, 0);
                if (result < 0)
                {
                    result = result * -1;
                }
                string resultToString = result.ToString();
                listOfInts.Add(resultToString);
            }
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
            //TODO: add sql query to disable the user
        }

        //Read the token given from the URL
        public bool checkResetIDIsValid(string resetID)
        {
           
            if (_resetRepo.existingResetIDGivenResetID(resetID))
            {
                DateTime when = _resetRepo.getExpirationTime(resetID);
                //Case where token hasm't expired
                if (!(when < DateTime.Now))
                {
                    return true;
                }
                //Case where token has expired, delete token
                else
                {
                    _resetRepo.deleteResetID(resetID);
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public void deleteResetIDFromDB(string resetID)
        {
            _resetRepo.deleteResetID(resetID);
        }



        #region Email Functions
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
            EmailService es = new EmailService();
            es.sendEmailPlainBody(userFullName, receiverEmail, resetPasswordSubjectString, resetPasswordBodyString);
        }

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
            EmailService es = new EmailService();
            es.sendEmailPlainBody(userFullName, receiverEmail, resetPasswordSubjectString, resetPasswordUserDoesNotExistEmailBody);
        }

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
            string resetPasswordBodyString = string.Format(template, data);
            EmailService es = new EmailService();
            es.sendEmailPlainBody(userFullName, receiverEmail, resetPasswordSubjectString, resetPasswordBodyString);
        }
        #endregion
    }
}
