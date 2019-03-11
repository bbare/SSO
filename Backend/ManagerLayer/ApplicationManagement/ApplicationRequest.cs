using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagerLayer.ApplicationManagement
{
    public class ApplicationRequest
    {
        public string Key { set; get; }
        public string Title { set; get; }
        public string Description { set; get; }
        public string LogoUrl { set; get; }
        public string LaunchUrl { set; get; }
        public string Email { set; get; }
        public string DeleteUrl { set; get; }
    }
}
