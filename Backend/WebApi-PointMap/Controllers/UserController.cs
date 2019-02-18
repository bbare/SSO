using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi_PointMap.Models;

namespace WebApi_PointMap.Controllers
{
    public class UserController : ApiController
    {
        // standard route is /api/user
        //  - verbs called on route determine the route pinged

        // GET api/User
        [HttpGet]
        [Route("api/user")]
        public IHttpActionResult Get()
        {
            // return OK with JSON
            var tester = new { name = "alfredo", vargas = "asdf"};
            return Ok(tester);
        }

        // GET api/User/5
        [HttpGet]
        [Route("api/user/{id}")] //route specific
        public IHttpActionResult Get(int id)
        {
            return Ok(new { id = id });
        }

        // POST api/User
        public IHttpActionResult Post(UserPOST value) //using a POCO to represent request
        {
            var response = value;
            return Ok(response);
        }

        // PUT api/User/5
        public void Put(int id, [FromBody]string value)
        {
            
        }

        // DELETE api/User/5
        public IHttpActionResult Delete(int id)
        {
            var response = new { id = id };
            return Ok(response);
        }

        // DELETE api/User
        public IHttpActionResult Delete(UserPOST user)
        {
            var response = new { id = user.Username };
            return Ok(response);
        }
    }
}
