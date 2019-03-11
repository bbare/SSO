using DataAccessLayer.Database;
using DataAccessLayer.Models;
using ManagerLayer.Login;
using ServiceLayer.Services;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Net;
using System.Web.Http;
using System.Web.Http.Cors;
using ManagerLayer;
using ServiceLayer.Exceptions;
using System.ComponentModel.DataAnnotations;

namespace KFC_WebAPI.Controllers
{
    public class UserRegistrationRequest
    {
        [Required]
        public string email { get; set; }
        [Required]
        public string password { get; set; }
        [Required]
        public DateTime dob { get; set; }
        [Required]
        public string city { get; set; }
        [Required]
        public string state { get; set; }
        [Required]
        public string country { get; set; }
        [Required]
        public string securityQ1 { get; set; }
        [Required]
        public string securityQ1Answer { get; set; }
        [Required]
        public string securityQ2 { get; set; }
        [Required]
        public string securityQ2Answer { get; set; }
        [Required]
        public string securityQ3 { get; set; }
        [Required]
        public string securityQ3Answer { get; set; }
    }

    public class UsersController : ApiController
    {
        [HttpPost]
        [Route("api/users/register")]
        public IHttpActionResult Register([FromBody]UserRegistrationRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            using (var _db = new DatabaseContext())
            {
                User user;
                try
                {
                    UserManager userManager = new UserManager();
                    user = userManager.CreateUser(
                        _db,
                        request.email,
                        request.password,
                        request.dob,
                        request.city,
                        request.state,
                        request.country,
                        request.securityQ1,
                        request.securityQ1Answer,
                        request.securityQ2,
                        request.securityQ2Answer,
                        request.securityQ3,
                        request.securityQ3Answer);
                } catch (ArgumentException ex)
                {
                    return Conflict();
                } catch (FormatException ex)
                {
                    return BadRequest("Invalid email address.");
                } catch (PasswordPwnedException ex)
                {
                    return Unauthorized();
                }

                AuthorizationManager authorizationManager = new AuthorizationManager();
                Session session = authorizationManager.CreateSession(_db, user);
                try
                {
                    _db.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    _db.Entry(user).State = System.Data.Entity.EntityState.Detached;
                    _db.Entry(session).State = System.Data.Entity.EntityState.Detached;
                    return InternalServerError();
                }

                return Ok(new
                {
                    data = new
                    {
                        token = user.Id
                    }
                });
            }
        }

        [HttpPost]
        [Route("api/users/login")]
        public IHttpActionResult Login([FromBody] LoginRequest request)
        {
            LoginManager loginM = new LoginManager();
            if (loginM.LoginCheckUserExists(request.email) == false)
            {
                return Content(HttpStatusCode.NotFound, "Invalid Username");
                //return NotFound();
            }
            else
            {
                if (loginM.LoginCheckUserDisabled())
                {
                    return Content(HttpStatusCode.Unauthorized, "User is Disabled");
                    //return Unauthorized();
                }
                else
                {
                    if (loginM.LoginCheckPassword(request.password))
                    {
                        return Ok(loginM.LoginAuthorized());
                    }
                    else
                    {
                        return Content(HttpStatusCode.Unauthorized, "Invalid Password");
                        //return Unauthorized();
                    }
                }
            }
        }
    }
}
