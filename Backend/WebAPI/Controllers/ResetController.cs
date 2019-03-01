using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ServiceLayer.Services;
using ManagerLayer.UserManagement;
using DataAccessLayer.Models;


namespace WebAPI.Controllers
{
    public class ResetController : ApiController
    {

        //After the user fills in the field with email, this action gets called
        [HttpPost]
        [ActionName("SendEmail")]
        public void sendResetEmail(string email)
        {
            ResetService rs = new ResetService();
            UserManagementManager umm = new UserManagementManager();
            //Check to see if the email provided is associated with an account
            if (umm.ExistingUser(email))
            {
                //Check to see if the account has a resetID already
                if (!rs.checkUserHasResetID(email)) //Don't have a resetID
                {
                    string resetID = rs.createResetID();
                    rs.addResetIDToDB(email, resetID);
                    string resetURL = rs.createResetURL(resetID);
                    rs.sendResetEmailUserExists(email, resetID);
                }
                else //Have a resetID
                {
                    //See how many resetIDs the email has
                    if (rs.getResetIDCount(email) < 4)
                    {
                        //Get the most recent resetID associated with the email provided
                        string resetIDAlreadyGivenToUser = rs.getResetID(email);
                        if (!rs.isLockedOut(resetIDAlreadyGivenToUser))//Check to see if the resetID isn't locked
                        {
                            if (!rs.isResetIDIsExpired(resetIDAlreadyGivenToUser)) //If the resetID given to the user is expired
                            {
                                //Delete the resetID from the DB
                                rs.deleteResetIDFromDB(resetIDAlreadyGivenToUser);
                                //Give the user a new resetID
                                string resetID = rs.createResetID();
                                rs.addResetIDToDB(email, resetID);
                                string resetURL = rs.createResetURL(resetID);
                                rs.sendResetEmailUserExists(email, resetID);
                            }
                            else
                            {
                                //Tell the user that to check their email
                            }
                        }
                        else
                        {
                            //get the expiration date of the token, and add 24 hours to it
                            //if 24 hours has passed since they failed the security questions
                                //delete all tokens
                            //else 
                                //Tell the user they cannot attempt to reset the password for 24 hours
                        }
                    }
                    else
                    {
                        // get the most recent token
                        //get the expiration time for that token
                        //add 24 hours to it
                        //check if that time is after the current time
                        //if the 24 hours hasn't passed

                            //Tell the user they cannot attempt to reset the password for 24 hours
                        //if the 24 hours has passed
                            //delete all tokens
                    }
                }
            }
            else
            {
                rs.sendResetEmailUserDoesNotExist(email);
            }
        }

        //After the user clicks the link in the email, this action gets called
        [HttpPost]
        [ActionName("ResetPassword")]
        public void resetPassword(string resetID)
        {
            ResetService rs = new ResetService();
            //Check to see if the resetID has been locked out 
            if (!rs.isLockedOut(resetID)) //ResetID is still valid
            {
                if (!rs.isResetIDIsExpired(resetID))
                {
                    //Check to see how many times they've attempted 
                    if (rs.getResetCount(resetID) == 3)
                    {
                        rs.lockResetID(resetID); //If the user has attempted to reset the password 3 times already with the same resetID, lock that resetID
                    }
                    else
                    {
                        //Get the security questions from the DB
                        rs.getSecurityQuestionsFromDB(resetID);
                    }
                }
                else
                {
                    rs.deleteResetIDFromDB(resetID);
                    //Tell the user the link has expired
                    //redirect to reset page
                }
            }
            else //resetID is locked out
            {
                //Tell the user they cannot attempt to reset the password for 24 hours
            }
        }
    }
}
