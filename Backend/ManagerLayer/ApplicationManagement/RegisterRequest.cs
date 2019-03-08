using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagerLayer.ApplicationManagement
{
    public class RegisterRequest
    {
        public string Title { set; get; }
        public string Url { set; get; }
        public string Email { set; get; }
        public string DeleteUrl { set; get; }
    }
}
