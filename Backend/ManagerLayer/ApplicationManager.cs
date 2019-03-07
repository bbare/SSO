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

namespace ManagerLayer
{
    public class ApplicationManager
    {
        public ApplicationManager()
        {

        }

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

        public bool IsValidUrl(string url, Uri uriResult)
        {
            bool result = Uri.TryCreate(url, UriKind.Absolute, out uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
            return result;
        }

        public Application CreateApplication(DatabaseContext _db, Application app)
        {
            // Aplication Service instance
            IApplicationService _appService = new ApplicationService();
            // Attempt to create an application record
            var appResponse = _appService.CreateApplication(_db, app);
            if (appResponse == null) // Application already exists
            {
                return null;
            }
            return appResponse;
        }

        public ApiKey CreateApiKey(DatabaseContext _db, ApiKey apiKey)
        {
            // ApiKey Service instance
            IApiKeyService _keyService = new ApiKeyService();
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
    }
}
