using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi_PointMap.Models
{
    // poco class for request object
    public class UserPOST
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}