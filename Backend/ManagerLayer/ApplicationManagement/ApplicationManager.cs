using DataAccessLayer.Database;
using DataAccessLayer.Models;
using ServiceLayer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ManagerLayer.ApplicationManagement
{
    public class ApplicationManager
    {
        public ApplicationManager()
        {

        }

        // Application Services
        private IApplicationService _appService = new ApplicationService();
        // ApiKey Service instance
        private IApiKeyService _keyService = new ApiKeyService();

        public HttpResponseContent ValidateRegistration(ApplicationRequest request)
        {
            HttpResponseContent response;
            Uri url = null;
            Uri deleteUrl = null;

            if (!IsValidEmail(request.Email))
            {
                // Error response
                response = new HttpResponseContent(HttpStatusCode.BadRequest, "Invalid Email");
                return response;
            }
            else if (!IsValidUrl(request.LaunchUrl, ref url))
            {
                // Error response
                response = new HttpResponseContent(HttpStatusCode.BadRequest, "Invalid Application Url");
                return response;
            }
            else if (!IsValidUrl(request.DeleteUrl, ref deleteUrl))
            {
                // Error response
                response = new HttpResponseContent(HttpStatusCode.BadRequest, "Invalid User Deletion Url");
                return response;
            }

            // Create a new application
            Application app = new Application
            {
                Title = request.Title,
                LaunchUrl = url.ToString(),
                Email = request.Email,
                UserDeletionUrl = request.DeleteUrl
            };

            // Create a new ApiKey
            ApiKey apiKey = new ApiKey
            {
                ApplicationId = app.Id
            };

            using (var _db = new DatabaseContext())
            {
                // Attempt to create an application record
                var appResponse = CreateApplication(_db, app);
                if (appResponse == null)
                {
                    response = new HttpResponseContent(HttpStatusCode.BadRequest, "Application Already Exists");
                    return response;
                }

                // Attempt to create an apiKey record
                var keyResponse = CreateApiKey(_db, apiKey);

                if (!SaveChanges(_db, appResponse, keyResponse))
                {
                    // Error response
                    response = new HttpResponseContent(HttpStatusCode.InternalServerError, "Unable to save database changes");
                    return response;
                }

                // Check if email was sent successfully
                //response = new HttpResponseContent(HttpStatusCode.BadRequest, "Unable to email API Key");
                //return response;
            }
            // Success: Return api key to frontend
            response = new HttpResponseContent(HttpStatusCode.OK, apiKey.Key);
            return response;
        }

        public HttpResponseContent ValidatePublish(ApplicationRequest request)
        {
            HttpResponseContent response;
            response = new HttpResponseContent(HttpStatusCode.OK, "Published to SSO");
            return response;
        }

        public HttpResponseContent ValidateKeyGeneration(ApplicationRequest request)
        {
            HttpResponseContent response;

            if (!IsValidEmail(request.Email))
            {
                // Error response
                response = new HttpResponseContent(HttpStatusCode.BadRequest, "Invalid Email");
                return response;
            }

            using (var _db = new DatabaseContext())
            {
                var app = GetApplication(_db, request.Title, request.Email);

                if(app == null)
                {
                    // Error response
                    response = new HttpResponseContent(HttpStatusCode.BadRequest, "Invalid Application");
                    return response;
                }

                // Create a new ApiKey
                ApiKey apiKey = new ApiKey
                {
                    ApplicationId = app.Id
                };

                // Attempt to create an apiKey record
                var keyResponse = CreateApiKey(_db, apiKey);

                if (!SaveChanges(_db, keyResponse))
                {
                    // Error response
                    response = new HttpResponseContent(HttpStatusCode.InternalServerError, "Unable to save database changes");
                    return response;
                }

                // Check if email was sent successfully
                //response = new HttpResponseContent(HttpStatusCode.BadRequest, "Unable to email API Key");
                //return response;

                response = new HttpResponseContent(HttpStatusCode.OK, apiKey.Key);
                return response;
            }
            
        }

        public bool IsValidEmail(string email)
        {
            try
            {
                // Check for a valid email.
                var valid = new System.Net.Mail.MailAddress(email);
                return true;
            }
            catch (Exception)
            {
                // Invalid email
                return false;
            }
        }

        public bool IsValidUrl(string url, ref Uri uriResult)
        {
            bool result = Uri.TryCreate(url, UriKind.Absolute, out uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
            return result;
        }

        public Application CreateApplication(DatabaseContext _db, Application app)
        {
            // Attempt to create an application record
            var appResponse = _appService.CreateApplication(_db, app);
            if (appResponse == null)
            {
                return null;
            }
            return appResponse;
        }

        public Application GetApplication(DatabaseContext _db, string title, string email)
        {
            var appResponse = _appService.GetApplication(_db, title, email);
            return appResponse;
        }

        public ApiKey CreateApiKey(DatabaseContext _db, ApiKey apiKey)
        {
            // Attempt to create an apiKey record
            var keyResponse = _keyService.CreateKey(_db, apiKey);
            return apiKey;
        }

        public bool SaveChanges(DatabaseContext _db, Application appResponse, ApiKey keyResponse)
        {
            try
            {
                // Save changes in the database
                _db.SaveChanges();

                // TODO: attempt to email api key instead of returning it to frontend
                //_keyService.EmailKey(app.Email, keyResponse.ToString());
                return true;
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException)
            {
                // Catch error
                // Detach application attempted to be created from the db context - rollback
                _db.Entry(appResponse).State = System.Data.Entity.EntityState.Detached;
                _db.Entry(keyResponse).State = System.Data.Entity.EntityState.Detached;

                // Error
                return false;
            }
        }

        public bool SaveChanges(DatabaseContext _db, ApiKey keyResponse)
        {
            try
            {
                // Save changes in the database
                _db.SaveChanges();

                // TODO: attempt to email api key instead of returning it to frontend
                //_keyService.EmailKey(app.Email, keyResponse.ToString());
                return true;
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException)
            {
                // Catch error
                // Detach api key attempted to be created from the db context - rollback
                _db.Entry(keyResponse).State = System.Data.Entity.EntityState.Detached;

                // Error
                return false;
            }
        }
    }
}
