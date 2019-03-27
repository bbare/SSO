using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using System.Web.Http.Cors;
using ManagerLayer.PasswordManagement;
using Newtonsoft.Json;

namespace WebAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
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
                string email = request.Email;
                if (email != null)
                {
                    PasswordManager pm = new PasswordManager();
                    string url = request.Url;
                    try
                    {
                        pm.SendResetToken(email, url);
                    }
                    catch (Exception ex)
                    {
                        return Content((HttpStatusCode)503, ex.Message);
                    }
                    return Content(HttpStatusCode.OK, "An email with further instructions has been sent");
                }
                return Content(HttpStatusCode.Unauthorized, "No email was provided");
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
                    List<string> userSubmittedSecurityAnswer = new List<string>
                {
                    request.SecurityA1,
                    request.SecurityA2,
                    request.SecurityA3
                };
                    if (pm.CheckSecurityAnswers(resetToken, userSubmittedSecurityAnswer))
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
                string submittedPassword = request.NewPassword;
                if (submittedPassword != null && submittedPassword.Length < 2001 && submittedPassword.Length > 11)
                {
                    PasswordManager pm = new PasswordManager();
                    if (pm.CheckPasswordResetValid(resetToken))
                    {
                        if (pm.CheckIfPasswordResetAllowed(resetToken))
                        {
                            if (!pm.CheckIsPasswordPwned(submittedPassword))
                            {
                                string newPasswordHashed = pm.SaltAndHashPassword(resetToken, submittedPassword);
                                pm.UpdatePassword(resetToken, newPasswordHashed);
                                return Content(HttpStatusCode.OK, "Password has been reset");
                            }
                            return Content(HttpStatusCode.BadRequest, "Password has been pwned, please use a different password");
                        }
                        return Content(HttpStatusCode.Unauthorized, "Reset password not allowed, answered security questions wrong too many times");
                    }
                    return Content(HttpStatusCode.Unauthorized, "Reset link is no longer valid");
                }
                return Content(HttpStatusCode.BadRequest, "Submitted password does not meet minimum requirements");
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, "Service Unavailable");
            }
        }
    }
}
