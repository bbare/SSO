using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using ManagerLayer.PasswordManagement;
using Newtonsoft.Json;

namespace WebAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ResetController : ApiController
    {
        public class EmailPost
        {
            [JsonProperty("email")]
            public string Email { get; set; }
        }

        public class SecurityAnswerPost
        {
            [JsonProperty("securityA1")]
            public string SecurityA1 { get; set; }
            [JsonProperty("securityA2")]
            public string SecurityA2 { get; set; }
            [JsonProperty("securityA3")]
            public string SecurityA3 { get; set; }
        }

        public class NewPasswordPost
        {
            [JsonProperty("NewPassword")]
            public string NewPassword { get; set; }
        }

        //After the user fills in the field with email, this action gets called
        [HttpPost]
        [Route("api/reset/send")]
        public HttpResponseMessage SendResetEmail([FromBody] EmailPost emailFromBody)
        {
            string email = emailFromBody.Email;
            if (email != null)
            {
                PasswordManager pm = new PasswordManager();
                string url = "localhost:8080/#/resetpassword/";
                pm.SendResetToken(email, url);
                var response = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent("An email with further instructions has been sent")
                };
                return response;
            }
            return Request.CreateResponse(HttpStatusCode.BadRequest, "No email was provided");
        }

        //After the user clicks the link in the email, this action gets called and takes the resetToken that's appended to the URL that was sent to the user
        [HttpGet]
        [Route("api/reset/{resetToken}")]
        public HttpResponseMessage Get(string resetToken)
        {
            PasswordManager pm = new PasswordManager();
            if (pm.CheckPasswordResetValid(resetToken))
            {
                return Request.CreateResponse(HttpStatusCode.OK, pm.GetSecurityQuestions(resetToken));
            }
            return Request.CreateResponse(HttpStatusCode.Unauthorized, "Reset link is no longer valid");
        }

        [HttpPost]
        [Route("api/reset/{resetToken}/checkanswers")]
        public HttpResponseMessage CheckAnswers(string resetToken, [FromBody] SecurityAnswerPost postedAnswers)
        {
            PasswordManager pm = new PasswordManager();
            if (pm.CheckPasswordResetValid(resetToken))
            {
                List<string> userSubmittedSecurityAnswer = new List<string>
                {
                    postedAnswers.SecurityA1,
                    postedAnswers.SecurityA2,
                    postedAnswers.SecurityA3
                };
                if (pm.CheckSecurityAnswers(resetToken, userSubmittedSecurityAnswer))
                {
                    return Request.CreateResponse(HttpStatusCode.OK, true);
                }
                return Request.CreateResponse(HttpStatusCode.BadRequest, false);
            }
            return Request.CreateResponse(HttpStatusCode.Unauthorized, "Reset link is no longer valid");
        }

        [HttpPost]
        [Route("api/reset/{resetToken}/resetpassword")]
        public HttpResponseMessage ResetPassword(string resetToken, [FromBody] NewPasswordPost passwordFromBody)
        {
            string submittedPassword = passwordFromBody.NewPassword;
            if (submittedPassword != null && submittedPassword.Length < 2001 && submittedPassword.Length > 11)
            {
                PasswordManager pm = new PasswordManager();
                if (pm.CheckPasswordResetValid(resetToken))
                {
                    if (pm.CheckIfPasswordResetAllowed(resetToken))
                    {
                        if (!pm.CheckIsPasswordPwned(submittedPassword))
                        {
                            string newPasswordHashed = pm.SaltAndHashPassword(submittedPassword);
                            pm.UpdatePassword(resetToken, newPasswordHashed);
                            return Request.CreateResponse(HttpStatusCode.OK, "Password has been reset");
                        }
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "Password has been pwned, please use a different password");
                    }
                    return Request.CreateResponse(HttpStatusCode.Unauthorized, "Reset password not allowed");
                }
                return Request.CreateResponse(HttpStatusCode.Unauthorized, "Reset link is no longer valid");
            }
            return Request.CreateResponse(HttpStatusCode.BadRequest, "Submitted password does not meet minimum requirements");
        }
    }
}
