using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ManagerLayer.ApplicationManagement
{
    public class HttpResponseContent
    {
        public HttpResponseContent(HttpStatusCode code, string message)
        {
            Code = code;
            Message = message;
        }

        public HttpStatusCode Code { get; set; }
        public string Message { get; set; }
    }
}
