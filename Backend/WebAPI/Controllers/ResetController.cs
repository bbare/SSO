using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebAPI.Controllers
{
    public class ResetController : ApiController
    {
        [HttpPost]
        public void sendResetEmail()
        {

        }

        [HttpPost]
        [ActionName("ResetPassword")]
        public void resetPassword(int id)
        {

        }
    }
}
