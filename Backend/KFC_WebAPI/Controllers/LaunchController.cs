using DataAccessLayer.Database;
using DataAccessLayer.Models;
using ManagerLayer;
using ManagerLayer.LaunchManagement;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Http;

namespace KFC_WebAPI.Controllers
{
    public class LaunchRequest
    {
        [Required]
        public string token { get; set; }
        [Required]
        public Guid appId { get; set; }
    }

    public class LaunchController : ApiController
    {
        [HttpGet]
        [Route("api/launch")]
        public IHttpActionResult Launch([FromUri, Required]LaunchRequest request)
        {
            using (var _db = new DatabaseContext())
            {
                IAuthorizationManager authorizationManager = new AuthorizationManager();
                Session session = authorizationManager.ValidateAndUpdateSession(_db, request.token);
                if (session == null)
                {
                    return Unauthorized();
                }

                ILaunchManager launchManager = new LaunchManager();
                LaunchResponse launchResponse = launchManager.SignLaunch(_db, session, request.appId);

                return Ok(launchResponse);
            }


        }
    }
}
