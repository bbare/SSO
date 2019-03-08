using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;
using DataAccessLayer.Database;
using DataAccessLayer.Models;
using Newtonsoft.Json;
using ServiceLayer.Services;
using ManagerLayer.ApplicationManagement;

namespace WebAPI.Controllers
{
    public class ApplicationController : ApiController
    {

        [HttpPost]
        [Route("api/applications/create")]
        public HttpResponseMessage Register([FromBody] ApplicationRequest request)
        {
            ManagerLayer.ApplicationManagement.ApplicationManager manager = new ManagerLayer.ApplicationManagement.ApplicationManager();
            HttpResponseContent responseContent = manager.ValidateRegistration(request);
            HttpResponseMessage response = Request.CreateResponse(responseContent.Code, responseContent.Message);
            
            return  response;
        }

        [HttpPost]
        [Route("api/applications/publish")]
        public HttpResponseMessage Publish([FromBody] ApplicationRequest request)
        {
            ManagerLayer.ApplicationManagement.ApplicationManager manager = new ManagerLayer.ApplicationManagement.ApplicationManager();
            HttpResponseContent responseContent = manager.ValidatePublish(request);
            HttpResponseMessage response = Request.CreateResponse(responseContent.Code, responseContent.Message);

            return response;
        }

        [HttpPost]
        [Route("api/applications/generatekey")]
        public HttpResponseMessage GenerateKey([FromBody] ApplicationRequest request)
        {
            ManagerLayer.ApplicationManagement.ApplicationManager manager = new ManagerLayer.ApplicationManagement.ApplicationManager();
            HttpResponseContent responseContent = manager.ValidateKeyGeneration(request);
            HttpResponseMessage response = Request.CreateResponse(responseContent.Code, responseContent.Message);

            return response;
        }

        [HttpOptions]
        public void Options(string locale, string deviceType)
        {
            // Add CORS headers to Options response
            string origin = HttpContext.Current.Request.Headers.Get("Origin") ?? "";
            HttpContext.Current.Response.AppendHeader("Access-Control-Allow-Origin", origin);
            HttpContext.Current.Response.AppendHeader("Access-Control-Allow-Methods", HttpContext.Current.Request.Headers["Access-Control-Request-Methods"]);
            HttpContext.Current.Response.AppendHeader("Access-Control-Allow-Headers", HttpContext.Current.Request.Headers["Access-Control-Request-Headers"]);
            HttpContext.Current.Response.End();
        }

    }
}