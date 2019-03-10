using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ServiceLayer.Services;
using DataAccessLayer.Models;
using ManagerLayer.PasswordManagement;
using ManagerLayer.UserManagement;
using Newtonsoft.Json;

namespace WebAPI.Controllers
{
    public class ResetController : ApiController
    {
        public class SecurityAnswerRequest
        {
            public string securityA1;
            public string securityA2;
            public string securityA3;
        }

        //IHttpActionResult
        //After the user fills in the field with email, this action gets called
        [HttpPost]
        [Route("api/reset/send")]
        public HttpResponseMessage SendResetEmail([FromBody]string email)
        {
            if (email != null)
            {
                PasswordManager pm = new PasswordManager();
                string url = Request.RequestUri.ToString();

                pm.AssignResetToken(email, url);
                var response = new HttpResponseMessage(HttpStatusCode.Created)
                {
                    Content = new StringContent("Email received")
                };
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

        }

        //After the user clicks the link in the email, this action gets called and takes the resetToken that's appended to the URL that was sent to the user
        [HttpGet]
        [Route("api/reset/{resetToken}")]
        public string Get(string resetToken)
        {
            PasswordManager pm = new PasswordManager();
            if (pm.CheckResetIDValid(resetToken))
            {
                return JsonConvert.SerializeObject(pm.GetSecurityQuestions(resetToken));
            }else
            {
                return null;
            }
        }

        [HttpPost]
        [Route("api/reset/{resetToken}/checkanswers")]
        public bool CheckAnswers(string resetToken, [FromBody] SecurityAnswerRequest request)
        {
            PasswordManager pm = new PasswordManager();
            if (pm.CheckResetIDValid(resetToken))
            {
                List<string> userSubmittedSecurityAnswer = new List<string>();
                userSubmittedSecurityAnswer.Add(request.securityA1);
                userSubmittedSecurityAnswer.Add(request.securityA2);
                userSubmittedSecurityAnswer.Add(request.securityA3);
                if (pm.CheckSecurityAnswers(resetToken, userSubmittedSecurityAnswer))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

        [HttpPost]
        [Route("api/reset/{resetToken}/resetpassword")]
        public bool ResetPassword(string resetToken, [FromBody] string newPasswordHash)
        {
            PasswordManager pm = new PasswordManager();
            if (pm.CheckResetIDValid(resetToken))
            {
                if (pm.CheckIfPasswordResetAllowed(resetToken))
                {
                    return pm.UpdatePassword(resetToken, newPasswordHash);
                }
            }
            return false;
        }
    }
}
