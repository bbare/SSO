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
        public HttpResponseMessage Register([FromBody] RegisterRequest request)
        {
            ManagerLayer.ApplicationManagement.ApplicationManager manager = new ManagerLayer.ApplicationManagement.ApplicationManager();
            HttpResponseContent responseContent = manager.ValidateApplication(request);
            HttpResponseMessage response = Request.CreateResponse(responseContent.Code, responseContent.Message);
            
            return  response;
        }

        // UNFINISHED
        // TODO: Convert to Register() format
        [HttpPost]
        [Route("api/applications/publish")]
        public async Task<string> Publish()
        {
            
            if (!Request.Content.IsMimeMultipartContent())
                return "No Mime Multipart Content";

            Application app = new Application();
            string key = "";

            var provider = new MultipartMemoryStreamProvider();
            await Request.Content.ReadAsMultipartAsync(provider);
            foreach (var file in provider.Contents)
            {
                //var contentType = file.Headers.ContentType.MediaType;

                //var filename = file.Headers.ContentDisposition.FileName.Trim('\"');
                //var buffer = await file.ReadAsByteArrayAsync();
                //Do whatever you want with filename and its binary data.
                //allHeaders += file.Headers.ToString();

                //return await file.ReadAsStringAsync();

                if (file.Headers.ContentDisposition.Name.Equals("title"))
                {
                    app.Title = await file.ReadAsStringAsync();
                }
                else if (file.Headers.ContentDisposition.Name.Equals("description"))
                {
                    app.Description = await file.ReadAsStringAsync();
                }
                else if (file.Headers.ContentDisposition.Name.Equals("logo"))
                {
                    app.LogoUrl = await file.ReadAsStringAsync();
                }
                else if (file.Headers.ContentDisposition.Name.Equals("key"))
                {
                    key = await file.ReadAsStringAsync();
                }
            }
            
            return "success";

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