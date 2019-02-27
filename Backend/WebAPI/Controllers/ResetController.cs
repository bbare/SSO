using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ServiceLayer.Services;

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
            if (userExistsinDB)
            {
                string resetToken = rs.createToken(email);
                rs.addTokenToDB(email, resetToken);
                string resetURL = rs.createResetURL(resetToken);
                rs.sendResetEmailUserExists(email, resetToken);
            }
            else
            {
                rs.sendResetEmailUserDoesNotExist(email);
            }
        }

        //After the user clicks the link in the email, this action gets called
        [HttpPost]
        [ActionName("ResetPassword")]
        public void resetPassword(string resetToken)
        {
            ResetService rs = new ResetService();
            if (rs.checkTokenIsValid(resetToken))
            {
                //Display Security Questions
            }
            else
            {
                //Tell the user the link has expired
                //redirect to reset page
            }
        }
    }
}
