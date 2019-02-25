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

        //Check DB for User
        public bool checkUser(string userName)
        {
            bool userExists = false;

            return userExists;
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

        //Function to add the token to the database
        public void addTokenToDB(string userName, string token, DateTime expirationTime)
        {

            //SQL Query to add 3 arguments to token table in DB
            //ResetRepository.addToken(userName, token, expirationTime);

            //sql query to disable the user
        }

        //Read the token given from the URL
        public bool checkTokenIsValid(string token)
        {
            bool tokenValid = false;
            byte[] data = Convert.FromBase64String(token);
            DateTime when = DateTime.FromBinary(BitConverter.ToInt64(data, 0));

            //Check to see if the token is valid
            //if ResetRepository.existingToken(token)
            //SQL Query to see if token exists in DB
                //get the byte data from token in db
                    //Compare the two's data to see if they're the same token

            //Case where token hasn't expired
            if(!(when < DateTime.Now))
            {
                tokenValid = true;
                ResetRepository.deleteToken(token);
                return tokenValid;
            }
            //Case where token has expired
            else
            {
                ResetRepository.deleteToken(token);
                return tokenValid;
            }
        }

        public void deleteTokenFromDB(string token)
        {
            ResetRepository.deleteToken(token)
        }

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
    }
}
