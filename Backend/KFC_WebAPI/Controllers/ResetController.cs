using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using System.Web.Http.Cors;
using ManagerLayer.PasswordManagement;
using Newtonsoft.Json;

namespace WebAPI.Controllers
{
    public class ResetController : ApiController
    {
        public class EmailRequest
        {
            [JsonProperty("email")]
            public string Email { get; set; }
            [JsonProperty("url")]
            public string Url { get; set; }
        }

        public class SecurityAnswerRequest
        {
            [JsonProperty("securityA1")]
            public string SecurityA1 { get; set; }
            [JsonProperty("securityA2")]
            public string SecurityA2 { get; set; }
            [JsonProperty("securityA3")]
            public string SecurityA3 { get; set; }
        }

        public class NewPasswordRequest
        {
            [JsonProperty("NewPassword")]
            public string NewPassword { get; set; }
        }

        //After the user fills in the field with email, this action gets called
        [HttpPost]
        [Route("api/reset/send")]
        public IHttpActionResult SendResetEmail([FromBody] EmailRequest request)
        {
            try
            {
                PasswordManager pm = new PasswordManager();
                int response = pm.SendResetEmail(request.Email, request.Url);
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
        public IHttpActionResult CheckAnswers(string resetToken, [FromBody] SecurityAnswerRequest request)
        {
            try
            {
                PasswordManager pm = new PasswordManager();
                if (pm.CheckPasswordResetValid(resetToken))
                {
                    List<string> userSubmittedSecurityAnswers = new List<string>
                {
                    request.SecurityA1,
                    request.SecurityA2,
                    request.SecurityA3
                };
                    if (pm.CheckSecurityAnswers(resetToken, userSubmittedSecurityAnswers))
                    {
                        return Content(HttpStatusCode.OK, true);
                    }
                    return Content(HttpStatusCode.BadRequest, false);
                }
                return Content(HttpStatusCode.Unauthorized, "Reset link is no longer valid");
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
                if(response == 1)
                {
                    return Content(HttpStatusCode.OK, "Password has been reset");
                }
                else if(response == -1)
                {
                    return Content(HttpStatusCode.BadRequest, "Password has been pwned, please use a different password");
                }
                else if(response == -2)
                {
                    return Content(HttpStatusCode.Unauthorized, "Reset password not allowed, answered security questions wrong too many times");
                }
                else if(response == -3)
                {
                    return Content(HttpStatusCode.Unauthorized, "Reset link is no longer valid");
                }
                else if(response == -4)
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
