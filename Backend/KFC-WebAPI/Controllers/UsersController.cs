using DataAccessLayer.Database;
using DataAccessLayer.DTOs;
using DataAccessLayer.Models;
using ServiceLayer.Services;
using ManagerLayer.Login;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace KFC_WebAPI.Controllers
{
    [EnableCors(origins: "http://localhost:8080", headers: "*", methods: "*")]
    public class UsersController : ApiController
    {
        public class RegisterRequest
        {
            public string email;
            public string password;
            public DateTime dob;
        }

        [HttpPost]
        [Route("api/users/register")]
        public string Register([FromBody] RegisterRequest request)
        {
            try
            {
                var valid = new System.Net.Mail.MailAddress(request.email);
            }
            catch (Exception)
            {
                return request.email;
                // TODO: Handle with REST error
            }
            IPasswordService _passwordService = new PasswordService();
            DateTime timestamp = DateTime.UtcNow;
            byte[] salt = _passwordService.GenerateSalt();
            string hash = _passwordService.HashPassword(request.password, salt);
            User user = new User
            {
                Email = request.email,
                PasswordHash = hash,
                PasswordSalt = salt,
                DateOfBirth = request.dob,
                UpdatedAt = timestamp
            };

            using (var _db = new DatabaseContext())
            {
                IUserService _userService = new UserService();
                var response = _userService.CreateUser(_db, user);
                try
                {
                    _db.SaveChanges();
                    return "woo it succeeded!";
                }
                catch (DbEntityValidationException ex)
                {
                    //catch error
                    // detach user attempted to be created from the db context - rollback
                    _db.Entry(response).State = System.Data.Entity.EntityState.Detached;
                }
            }
            return "got to end without stuff";
        }

        /// <summary>
        /// NOT DONE. STILL IN PROCESS
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/users/login")]
        public IHttpActionResult Login([FromBody] LoginRequest request)
        {           
            LoginManager loginM = new LoginManager();
            if (loginM.LoginCheckUserExists(request.email) == false)
            {
                return NotFound();
            }
            else
            {
                if(loginM.LoginCheckUserDisabled())
                {
                    return Unauthorized();
                }
                else
                {
                    if(loginM.LoginCheckPassword(request.password))
                    {
                        return Ok(loginM.LoginAuthorized());
                    }
                    else
                    {
                        return Unauthorized();
                    }
                }
            }            
        }
    }
}
