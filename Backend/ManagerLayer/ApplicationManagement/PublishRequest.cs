using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagerLayer.ApplicationManagement
{
    public class PublishRequest
    {
        public string Key { set; get; }
        public string Title { set; get; }
        public string Description { set; get; }
        public string Logo { set; get; }
    }
}
