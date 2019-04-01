using System;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using DataAccessLayer.Database;
using DataAccessLayer.Models;
using ManagerLayer;



namespace KFC_WebAPI.Controllers
{
    public class LogoutRequest
    {
        [Required]
        public string token { get; set; }
        [Required]
        public Guid appId { get; set; }
    }
    public class LogoutController : ApiController
    {

        
        [HttpPost]
        [Route("api/Logout")]
        public void DeleteSession([FromBody] LogoutRequest request)
        {
            using (var _db = new DatabaseContext())
            {
                IAuthorizationManager authorizationManager = new AuthorizationManager();
                Session session = authorizationManager.DeleteSession(_db, request.token);

            }

        }

    }
}