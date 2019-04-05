using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Infrastructure;
using System.Net;
using System.Web.Http;
using DataAccessLayer.Database;
using DataAccessLayer.Models;
using ManagerLayer;
using ServiceLayer.Services;



namespace KFC_WebAPI.Controllers
{


    public class LogoutRequest
    {
        [Required]
        public string token { get; set; }
    }

    public class LogoutController : ApiController
    {
        
        [HttpPost]
        [Route("api/Logout")]
        public IHttpActionResult DeleteSession([FromBody] LogoutRequest request)
        {
            SessionService serv = new SessionService();
            using (var _db = new DatabaseContext())
            {
                IAuthorizationManager authorizationManager = new AuthorizationManager();
                
              
                
                try
                {
                    var response = authorizationManager.DeleteSession(_db, request.token);
                    _db.SaveChanges();
                    
                    if (response != null)
                    {
                        
                        return Ok("User has logged out");
                    }

                }
                catch (DbUpdateException)
                {
                    return Content(HttpStatusCode.InternalServerError, "There was an error on the server and the request could not be completed");
                }
                return Content(HttpStatusCode.ExpectationFailed, "Session has not been found.");

            }

        }

    }
}

