using DataAccessLayer.Database;
using DataAccessLayer.Models;
using ManagerLayer.Login;
using System;
using System.Data.Entity.Validation;
using System.Net;
using System.Web.Http;
using ManagerLayer;
using ServiceLayer.Exceptions;
using System.ComponentModel.DataAnnotations;
using ManagerLayer.PasswordManagement;
using ManagerLayer.UserManagement;
using ServiceLayer.Services;

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

    public class UpdatePasswordRequest
    {
        [Required]
        public string emailAddress { get; set; }
        [Required]
        public string sessionToken { get; set; }
        [Required]
        public string oldPassword { get; set; }
        [Required]
        public string NewPassword { get; set; }
    }

    public class UsersController : ApiController
    {
        [HttpPost]
        [Route("api/users/register")]
        public IHttpActionResult Register([FromBody, Required]UserRegistrationRequest request)
        {
            if (!ModelState.IsValid || request == null)
            {
                return Content((HttpStatusCode)412, ModelState);
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
                } catch (ArgumentException)
                {
                    return Conflict();
                } catch (FormatException)
                {
                    return Content((HttpStatusCode)406, "Invalid email address.");
                }
                catch (PasswordInvalidException)
                {
                    return Content((HttpStatusCode)401, "That password is too short. Password must be between 12 and 2000 characters.");
                }
                catch (PasswordPwnedException)
                {
                    return Content((HttpStatusCode)401, "That password has been hacked before. Please choose a more secure password.");
                }
                catch (InvalidDobException)
                {
                    return Content((HttpStatusCode)401, "This software is intended for persons over 18 years of age.");
                }

                AuthorizationManager authorizationManager = new AuthorizationManager();
                Session session = authorizationManager.CreateSession(_db, user);
                try
                {
                    _db.SaveChanges();
                }
                catch (DbEntityValidationException)
                {
                    _db.Entry(user).State = System.Data.Entity.EntityState.Detached;
                    _db.Entry(session).State = System.Data.Entity.EntityState.Detached;
                    return InternalServerError();
                }

                return Ok(new
                {
                    token = session.Token
                });
            }
        }

        [HttpPost]
        [Route("api/users/login")]
        public IHttpActionResult Login([FromBody] LoginRequest request)
        {
            LoginManager loginM = new LoginManager();
            if (loginM.LoginCheckUserExists(request) == false)
            {
                //404
                return Content(HttpStatusCode.NotFound, "Invalid Username");
                //return NotFound();
            }
            else
            {
                if (loginM.LoginCheckUserDisabled(request))
                {
                    //401
                    return Content(HttpStatusCode.Unauthorized, "User is Disabled");
                    //return Unauthorized();
                }
                else
                {
                    if (loginM.LoginCheckPassword(request))
                    {
                        return Ok(loginM.LoginAuthorized());
                    }
                    else
                    {
                        //400
                        return Content(HttpStatusCode.BadRequest, "Invalid Password");
                    }
                }
            }
        }

        [HttpPost]
        [Route("api/users/updatepassword")]
        public IHttpActionResult UpdatePassword([FromBody] UpdatePasswordRequest request)
        {
            using(var _db = new DatabaseContext())
            {
                UserManagementManager umm = new UserManagementManager();
                User retrievedUser = umm.GetUser(request.emailAddress);
                if (retrievedUser != null)
                {
                    SessionService ss = new SessionService();
                    Session session = ss.GetSession(_db, request.sessionToken);
                    if (session != null)
                    {
                        PasswordManager pm = new PasswordManager();
                        string oldPasswordHashed = pm.SaltAndHashPassword(request.oldPassword);
                        if (oldPasswordHashed != retrievedUser.PasswordHash)
                        {
                            if (request.NewPassword.Length >= 12 || request.NewPassword.Length <= 2000)
                            {
                                string newPasswordHashed = pm.SaltAndHashPassword(request.NewPassword);
                                if (pm.UpdatePassword(retrievedUser, request.NewPassword))
                                {
                                    return Content(HttpStatusCode.OK, "Password has been updated");
                                }
                                return Content(HttpStatusCode.BadRequest, "New password matches old password");
                            }
                            return Content(HttpStatusCode.BadRequest, "New password does not meet minimum password requirements");
                        }
                        return Content(HttpStatusCode.BadRequest, "Current password inputted does not match current password");
                    }
                    return Content(HttpStatusCode.Unauthorized, "Session invalid");
                }
                return Content(HttpStatusCode.BadRequest, "Email address is not valid");
            }
        }
    }
}