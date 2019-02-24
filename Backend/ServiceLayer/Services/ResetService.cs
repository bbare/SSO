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
            //sql query to disable the user
        }

        //Read the token given from the URL
        public bool checkTokenIsValid(string token)
        {
            bool tokenValid = false;
            byte[] data = Convert.FromBase64String(token);
            DateTime when = DateTime.FromBinary(BitConverter.ToInt64(data, 0));

            //Check to see if the token is valid
            
            //SQL Query to see if token exists in DB
                //get the byte data from token in db
                    //Compare the two's data to see if they're the same token

            //Case where token hasn't expired
            if(!(when < DateTime.Now))
            {
                tokenValid = true;
                deleteTokenFromDB();
                return tokenValid;
            }
            //Case where token has expired
            else
            {
                deleteTokenFromDB();
                return tokenValid;
            }
        }

        public void deleteTokenFromDB()
        {

        }

        public void sendResetEmailUserExists()
        {

        }
        public void sendResetEmailUserDoesNotExist()
        {

        }
    }
}
