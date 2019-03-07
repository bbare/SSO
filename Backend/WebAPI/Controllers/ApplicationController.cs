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
using ManagerLayer;

namespace WebAPI.Controllers
{
    public class ApplicationController : ApiController
    {
        /// <summary>
        /// Application fields received in Register POST request.
        /// </summary>
        public class RegisterRequest
        {
            public string title;
            public string url;
            public string email;
            public string deleteUrl;
        }

        /// <summary>
        /// Application and ApiKey fields received in Publish POST request.
        /// </summary>
        public class PublishRequest
        {
            public string key;
            public string title;
            public string description;
            public string logo;
        }

        [HttpPost]
        [Route("api/applications/register")]
        public HttpResponseMessage Register([FromBody] RegisterRequest request)
        {
            HttpResponseMessage response ;
            ManagerLayer.ApplicationManager manager = new ManagerLayer.ApplicationManager();
            Uri url = null;
            Uri deleteUrl = null;

            if (!manager.IsValidEmail(request.email))
            {
                // Error response
                response = Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid Email");
                return response;
            }
            else if (!manager.IsValidUrl(request.url, url))
            {
                // Error response
                response = Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid Application Url");
                return response;
            }
            else if (!manager.IsValidUrl(request.deleteUrl, deleteUrl))
            {
                // Error response
                response = Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid User Deletion Url");
                return response;
            }

            // Create a new application
            Application app = new Application
            {
                Title = request.title,
                LaunchUrl = url.ToString(),
                Email = request.email,
                UserDeletionUrl = request.deleteUrl
            };

            // Create a new ApiKey
            ApiKey apiKey = new ApiKey
            {
                ApplicationId = app.Id
            };

            using (var _db = new DatabaseContext())
            {
                // Attempt to create an application record
                var appResponse = manager.CreateApplication(_db, app);
                if (appResponse == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.BadRequest, "Application Already Exists");
                    return response;
                }
                
                // Attempt to create an apiKey record
                var keyResponse = manager.CreateApiKey(_db, apiKey);

                if(!manager.SaveChanges(_db, appResponse, keyResponse))
                {
                    // Error response
                    response = Request.CreateResponse(HttpStatusCode.BadRequest, "Unable to save database changes");
                    return response;
                }

                // Check if email was sent successfully
                //response = Request.CreateResponse(HttpStatusCode.BadRequest, "Unable to email API Key");
                //return response;
            }
            // Success: Return api key to frontend
            response = Request.CreateResponse(HttpStatusCode.OK, apiKey.Key);
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