using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApi_PointMap.Controllers
{
    public class PointController : ApiController
    {
        // GET api/point/get
        [HttpGet]
        [Route("point/get")]
        public IHttpActionResult Get()
        {
            return Ok("point");
        }
    }
}
