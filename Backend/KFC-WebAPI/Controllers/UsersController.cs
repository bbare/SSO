using DataAccessLayer.Database;
using DataAccessLayer.DTOs;
using DataAccessLayer.Models;
using ServiceLayer.Services;
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
            //WILL MOVE TO MANAGER LAYER SOON
            IUserService _userService = new UserService();
            IPasswordService _passwordService = new PasswordService();
            ITokenService _tokenService = new TokenService();
            ISessionService _sessionService = new SessionService();

            string generateToken = _tokenService.GenerateToken();

            using (var _db = new DatabaseContext())
            {
                User user = _userService.GetUser(_db, request.email);
                bool isPasswordMatched = _passwordService.VerifyPassword(request.password, user.PasswordHash, user.PasswordSalt);

                //succesful login
                if (isPasswordMatched == true)
                {
                    Session session = new Session
                    {
                        Token = generateToken,
                        User = user
                    };

                    var response = _sessionService.CreateSession(_db,session);

                    return Ok(response);
         
                }
                //login not successful if password is incorrect
                else
                {
                    user.IncorrectPasswordCount = user.IncorrectPasswordCount + 1;

                    if(user.IncorrectPasswordCount == 3)
                    {
                        user.Disabled = true;
                    }

                    return NotFound();
                }
            }
        }

    }
}
