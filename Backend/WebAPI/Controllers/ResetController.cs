using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ServiceLayer.Services;
using DataAccessLayer.Models;
using ManagerLayer.ResetPassword;
using ManagerLayer.UserManagement;

namespace WebAPI.Controllers
{
    public class ResetController : ApiController
    {

        //After the user fills in the field with email, this action gets called
        [HttpPost]
        [ActionName("Send")]
        public void SendResetEmail(string email)
        {
            PasswordResetManager prm = new PasswordResetManager();
            UserManagementManager umm = new UserManagementManager();
            //Check to see if the email provided is associated with an account
            if (umm.ExistingUser(email))
            {
                if (umm.GetUser(email).numOfResetAttempts != 3)//Check to see if the user is unable to request new reset links
                {

                    var resetID = prm.CreatePasswordReset(email);
                    prm.SendResetEmailUserExists(email, resetID.resetID);
                }

            }
            else
            {
                prm.SendResetEmailUserDoesNotExist(email);
            }
        }

        //After the user clicks the link in the email, this action gets called
        [HttpPost]
        public void Post(string resetID)
        {
            PasswordResetManager prm = new PasswordResetManager();
            if (prm.checkResetIDValid(resetID))
            {
                prm.GetSecurityQuestions(resetID);
            }
        }
    }
}
