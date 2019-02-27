using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Database;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories;


namespace ServiceLayer.Services
{
    class ResetService
    {
        //Variable for how long the token is supposed to be live
        private const double TimeToExpire = 5;
        //Constant for the password reset email subject line
        private const string resetPasswordSubjectString = "KFC SSO Password Reset";

        private const string resetControllerURL = "kfcsso.com/api/reset";

        private ResetRepository _resetRepo;
        public ResetService()
        {
            _resetRepo = new ResetRepository(new DatabaseContext());
        }

        //Function to create token in the system
        public string createToken(string userName)
        {
            //Gets current time, and adds the time to expire constant
            DateTime expirationTime = DateTime.Now.AddMinutes(TimeToExpire);

            byte[] time = BitConverter.GetBytes(expirationTime.ToBinary());
            byte[] key = Guid.NewGuid().ToByteArray();

            string token = Convert.ToBase64String(time.Concat(key).ToArray());

            addTokenToDB(userName, token, expirationTime);

            return token;
        }

        public string createResetURL(string token)
        {
            string resetURL = ;
            return resetURL;
        }

        //Function to add the token to the database
        public void addTokenToDB(string userName, string token, DateTime expirationTime)
        {
            _resetRepo.addToken(userName, token, expirationTime);
            //TODO: add sql query to disable the user
        }

        //Read the token given from the URL
        public bool checkTokenIsValid(string token)
        {
            byte[] dataFromURL = Convert.FromBase64String(token);
            if (_resetRepo.existingTokenGivenToken(token))
            {
                DateTime when = _resetRepo.getExpirationTime(token);
                //Case where token hasm't expired
                if (!(when < DateTime.Now))
                {
                    return true;
                }
                //Case where token has expired, delete token
                else
                {
                    _resetRepo.deleteToken(token);
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public void deleteTokenFromDB(string token)
        {
            _resetRepo.deleteToken(token);
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

            EmailService.sendEmailPlainBody(userFullName, receiverEmail, resetPasswordSubjectString, resetPasswordBodyString);
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
            EmailService.sendEmailPlainBody(userFullName, receiverEmail, resetPasswordSubjectString, resetPasswordUserDoesNotExistEmailBody);
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

            EmailService.sendEmailPlainBody(userFullName, receiverEmail, resetPasswordSubjectString, resetPasswordBodyString);
        }
        #endregion
    }
}
