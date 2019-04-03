using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using DataAccessLayer.Requests;
using ManagerLayer.PasswordManagement;
namespace WebAPI.Controllers
{
    public class ResetController : ApiController
    {
        //After the user fills in the field with email, this action gets called
        [HttpPost]
        [Route("api/reset/send")]
        public IHttpActionResult SendResetEmail([FromBody] SendResetEmailRequest request)
        {
            try
            {
                PasswordManager pm = new PasswordManager();
                int response = pm.SendResetEmail(request.email, request.url);
                if (response == -1)
                {
                    return Content((HttpStatusCode)503, "Email service unavailable");
                }
                else if (response == 0)
                {
                    return Content(HttpStatusCode.Unauthorized, "No email was provided");
                }
                else if (response == 1)
                {
                    return Content(HttpStatusCode.OK, "An email with further instructions has been sent");
                }
                else
                {
                    return Content(HttpStatusCode.BadRequest, "Service Unavailable");
                }
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, "Service Unavailable");
            }
        }

        //After the user clicks the link in the email, this action gets called and takes the resetToken that's appended to the URL that was sent to the user
        [HttpGet]
        [Route("api/reset/{resetToken}")]
        public IHttpActionResult Get(string resetToken)
        {
            try
            {
                PasswordManager pm = new PasswordManager();
                if (pm.CheckPasswordResetValid(resetToken))
                {
                    return Content(HttpStatusCode.OK, pm.GetSecurityQuestions(resetToken));
                }
                return Content(HttpStatusCode.Unauthorized, "Reset link is no longer valid");
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, "Service Unavailable");
            }
        }

        [HttpPost]
        [Route("api/reset/{resetToken}/checkanswers")]
        public IHttpActionResult CheckAnswers(string resetToken, [FromBody] SecurityAnswersRequest request)
        {
            try
            {
                PasswordManager pm = new PasswordManager();
                int response = pm.CheckSecurityAnswersController(resetToken, request);
                if (response == 1)
                {
                    return Content(HttpStatusCode.OK, true);
                }
                else if (response == -1)
                {
                    return Content(HttpStatusCode.BadRequest, "One or more of the answers inputted are incorrect");
                }
                else if (response == -2)
                {
                    return Content(HttpStatusCode.Unauthorized, "Reset link is no longer valid");
                }
                else { return Content(HttpStatusCode.BadRequest, "Service Unavailable"); }
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, "Service Unavailable");
            }
        }

        [HttpPost]
        [Route("api/reset/{resetToken}/resetpassword")]
        public IHttpActionResult ResetPassword(string resetToken, [FromBody] NewPasswordRequest request)
        {
            try
            {
                PasswordManager pm = new PasswordManager();
                int response = pm.ResetPasswordController(resetToken, request.NewPassword);
                if (response == 1)
                {
                    return Content(HttpStatusCode.OK, "Password has been reset");
                }
                else if (response == -1)
                {
                    return Content(HttpStatusCode.BadRequest, "Password has been pwned, please use a different password");
                }
                else if (response == -2)
                {
                    return Content(HttpStatusCode.Unauthorized, "Reset password not allowed, answered security questions wrong too many times");
                }
                else if (response == -3)
                {
                    return Content(HttpStatusCode.Unauthorized, "Reset link is no longer valid");
                }
                else if (response == -4)
                {
                    return Content(HttpStatusCode.BadRequest, "Submitted password does not meet minimum requirements");
                }
                else
                {
                    return Content(HttpStatusCode.BadRequest, "Service Unavailable");
                }
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, "Service Unavailable");
            }
        }
    }
}
