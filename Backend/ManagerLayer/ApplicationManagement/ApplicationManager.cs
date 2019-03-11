using DataAccessLayer.Database;
using DataAccessLayer.Models;
using MimeKit;
using ServiceLayer.Services;
using System;
using System.Drawing;
using System.IO;
using System.Net;

namespace ManagerLayer.ApplicationManagement
{
    public class ApplicationManager
    {
        public ApplicationManager()
        {
            _appService = new ApplicationService();
            _keyService = new ApiKeyService();
            //_emailService = new EmailService();
            _tokenService = new TokenService();
        }

        // Services
        private IApplicationService _appService;
        private IApiKeyService _keyService;
        private IEmailService _emailService;
        private ITokenService _tokenService;

        /// <summary>
        /// Validate the app registration field values.
        /// </summary>
        /// <param name="request">Values from POST request</param>
        /// <returns>Http status code and message</returns>
        public HttpResponseContent ValidateRegistration(ApplicationRequest request)
        {
            // Http status code and message
            HttpResponseContent response;
            Uri launchUrl = null;
            Uri deleteUrl = null;

            // Validate values
            if (!IsValidTitle(request.Title))
            {
                // Error response
                response = new HttpResponseContent(HttpStatusCode.BadRequest, "Invalid Title: Cannot be more than 100 characters in length.");
                return response;
            }
            else if (!IsValidEmail(request.Email))
            {
                // Error response
                response = new HttpResponseContent(HttpStatusCode.BadRequest, "Invalid Email");
                return response;
            }
            else if (!IsValidUrl(request.LaunchUrl, ref launchUrl))
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
                LaunchUrl = launchUrl.ToString(),
                Email = request.Email,
                UserDeletionUrl = request.DeleteUrl
            };

            // Create a new ApiKey
            ApiKey apiKey = new ApiKey
            {
                Key = _tokenService.GenerateToken(),
                ApplicationId = app.Id
            };

            using (var _db = new DatabaseContext())
            {
                // Attempt to create an application record
                var appResponse = _appService.CreateApplication(_db, app);
                if (appResponse == null)
                {
                    // Error response
                    response = new HttpResponseContent(HttpStatusCode.BadRequest, "Application Already Exists");
                    return response;
                }

                // Attempt to create an apiKey record
                var keyResponse = _keyService.CreateKey(_db, apiKey);

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
            string message = apiKey.Key;
            //string message;

            //// Attempt to send api key to application email
            //if(SendAppRegistrationApiKeyEmail(app.Email, apiKey.Key))
            //{
            //    // Alert front end that email was sent
            //    message = "Sent to " + app.Email;
            //}
            //else
            //{
            //    // Email could not be sent. Send api key to frontend.
            //    message = apiKey.Key;
            //}


            // Return success messge
            response = new HttpResponseContent(HttpStatusCode.OK, message);
            return response;
        }

        /// <summary>
        /// Validate the app publish field values.
        /// </summary>
        /// <param name="request">Values from POST request</param>
        /// <returns>Http status code and message</returns>
        public HttpResponseContent ValidatePublish(ApplicationRequest request)
        {
            // Http status code and message
            HttpResponseContent response;
            Uri logoUrl = null;

            // Validate values
            if (!IsValidDescription(request.Description))
            {
                // Error response
                response = new HttpResponseContent(HttpStatusCode.BadRequest, "Invalid Description: Cannot be more than 2000 characters in length.");
                return response;
            }
            else if (!IsValidUrl(request.LogoUrl, ref logoUrl))
            {
                // Error response
                response = new HttpResponseContent(HttpStatusCode.BadRequest, "Invalid Logo Url.");
                return response;
            }
            else if (!IsValidImageExtension(logoUrl))
            {
                // Error response
                response = new HttpResponseContent(HttpStatusCode.BadRequest, "Invalid Logo Image Extension: Can only be .PNG");
                return response;
            }
            else if (!IsValidDimensions(logoUrl))
            {
                // Error response
                response = new HttpResponseContent(HttpStatusCode.BadRequest, "Invalid Logo Dimensions: Can be no more than 55x55 pixels.");
                return response;
            }

            using (var _db = new DatabaseContext())
            {
                // Attempt to find api key
                var apiKey = _keyService.GetKey(_db, request.Key);

                // Key must exist and be unused.
                if(apiKey == null || apiKey.IsUsed == true)
                {
                    // Error response
                    response = new HttpResponseContent(HttpStatusCode.BadRequest, "Invalid Key");
                    return response;
                }

                // Attempt to get application based on ApplicationId from api key
                var app = _appService.GetApplication(_db, apiKey.ApplicationId);
                // Published application title is used to authenticate the app.
                if (app == null || !request.Title.Equals(app.Title))
                {
                    // Error response
                    response = new HttpResponseContent(HttpStatusCode.BadRequest, "Invalid Key");
                    return response;
                }

                // Update values of application record
                app.Description = request.Description;
                app.LogoUrl = request.LogoUrl;
                var appResponse = _appService.UpdateApplication(_db, app);

                // Update values of api key record
                apiKey.IsUsed = true;
                var keyResponse = _keyService.UpdateKey(_db, apiKey);
                
                // Attempt to save database changes
                if (!SaveChanges(_db, appResponse, keyResponse))
                {
                    // Error response
                    response = new HttpResponseContent(HttpStatusCode.InternalServerError, "Unable to save database changes");
                    return response;
                }

                response = new HttpResponseContent(HttpStatusCode.OK, "Published to KFC SSO");
                return response;
            }
            
        }

        /// <summary>
        /// Validate the key generation field values.
        /// </summary>
        /// <param name="request">Values from POST request</param>
        /// <returns>Http status code and message</returns>
        public HttpResponseContent ValidateKeyGeneration(ApplicationRequest request)
        {
            // Http status code and message
            HttpResponseContent response;

            // Validate values
            if (!IsValidEmail(request.Email))
            {
                // Error response
                response = new HttpResponseContent(HttpStatusCode.BadRequest, "Invalid Email");
                return response;
            }

            using (var _db = new DatabaseContext())
            {
                // Attempt to find application
                var app = _appService.GetApplication(_db, request.Title, request.Email);
                if(app == null)
                {
                    // Error response
                    response = new HttpResponseContent(HttpStatusCode.BadRequest, "Invalid Application");
                    return response;
                }

                // Create a new ApiKey
                ApiKey apiKey = new ApiKey
                {
                    Key = _tokenService.GenerateToken(),
                    ApplicationId = app.Id
                };

                // Attempt to create an apiKey record
                var keyResponse = _keyService.CreateKey(_db, apiKey);

                if (!SaveChanges(_db, keyResponse))
                {
                    // Error response
                    response = new HttpResponseContent(HttpStatusCode.InternalServerError, "Unable to save database changes");
                    return response;
                }

                string message = apiKey.Key;
                //string message;

                //// Attempt to send api key to application email
                //if (SendNewApiKeyEmail(app.Email, apiKey.Key))
                //{
                //    // Alert front end that email was sent
                //    message = "Sent to " + app.Email;
                //}
                //else
                //{
                //    // Email could not be sent. Send api key to frontend.
                //    message = apiKey.Key;
                //}

                response = new HttpResponseContent(HttpStatusCode.OK, apiKey.Key);
                return response;
            }
            
        }

        public HttpResponseContent ValidateDeletion(ApplicationRequest request)
        {
            // Http status code and message
            HttpResponseContent response;

            // Validate values
            if (!IsValidEmail(request.Email))
            {
                // Error response
                response = new HttpResponseContent(HttpStatusCode.BadRequest, "Invalid Email");
                return response;
            }

            using (var _db = new DatabaseContext())
            {
                // Attempt to find application
                var app = _appService.GetApplication(_db, request.Title, request.Email);
                if (app == null)
                {
                    // Error response
                    response = new HttpResponseContent(HttpStatusCode.BadRequest, "Invalid Application");
                    return response;
                }

                // Attempt to create an apiKey record
                var appResponse = _appService.DeleteApplication(_db, app.Id);

                if (!SaveChanges(_db, appResponse))
                {
                    // Error response
                    response = new HttpResponseContent(HttpStatusCode.InternalServerError, "Unable to save database changes");
                    return response;
                }

                response = new HttpResponseContent(HttpStatusCode.OK, "Application Deleted from KFC SSO");
                return response;
            }
        }

        /// <summary>
        /// Validates title length
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public bool IsValidTitle(string title)
        {
            int length = title.Length;

            // Application title cannot be more than 100 characters.
            if(length > 100)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Validates email format
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Validates url format
        /// </summary>
        /// <param name="url"></param>
        /// <param name="uriResult"></param>
        /// <returns></returns>
        public bool IsValidUrl(string url, ref Uri uriResult)
        {
            bool result = Uri.TryCreate(url, UriKind.Absolute, out uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
            return result;
        }

        /// <summary>
        /// Validates description length
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public bool IsValidDescription(string description)
        {
            int length = description.Length;

            // Application description cannot be more than 2000 characters.
            if (length > 2000)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Validates image file type.
        /// </summary>
        /// <param name="logoUrl"></param>
        /// <returns></returns>
        public bool IsValidImageExtension(Uri logoUrl)
        {
            string extension = Path.GetExtension("@" + logoUrl.ToString());

            // Logo can only be of .PNG image file type.
            if (!extension.ToUpper().Equals(".PNG"))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Validates image dimensions
        /// </summary>
        /// <param name="logoUrl"></param>
        /// <returns></returns>
        public bool IsValidDimensions(Uri imgUrl)
        {
            // Download image
            WebClient wc = new WebClient();
            byte[] bytes = wc.DownloadData(imgUrl);
            MemoryStream ms = new MemoryStream(bytes);
            Image img = Image.FromStream(ms);

            // Get dimensions
            int x, y;
            x = img.Width;
            y = img.Height;

            // Logo dimensions can be no more than 55x55 pixels
            if(x > 55 || y > 55)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Saves the changes made to the database tables
        /// </summary>
        /// <param name="_db"></param>
        /// <param name="appResponse">Application change</param>
        /// <param name="keyResponse">Api key change</param>
        /// <returns>Whether the save was successful</returns>
        public bool SaveChanges(DatabaseContext _db, Application appResponse, ApiKey keyResponse)
        {
            try
            {
                // Save changes in the database
                _db.SaveChanges();
                
                return true;
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException)
            {
                // Catch error
                // Detach application and api key attempted to be changed from the db context - rollback
                _db.Entry(appResponse).State = System.Data.Entity.EntityState.Detached;
                _db.Entry(keyResponse).State = System.Data.Entity.EntityState.Detached;

                // Error
                return false;
            }
        }

        /// <summary>
        /// Saves the changes made to the database tables
        /// </summary>
        /// <param name="_db">database</param>
        /// <param name="keyResponse">Api key change</param>
        /// <returns>Whether the change was successful</returns>
        public bool SaveChanges(DatabaseContext _db, ApiKey keyResponse)
        {
            try
            {
                // Save changes in the database
                _db.SaveChanges();
                
                return true;
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException)
            {
                // Catch error
                // Detach api key attempted to be changed from the db context - rollback
                _db.Entry(keyResponse).State = System.Data.Entity.EntityState.Detached;

                // Error
                return false;
            }
        }

        /// <summary>
        /// Saves the changes made to the database tables
        /// </summary>
        /// <param name="_db">database</param>
        /// <param name="appResponse">Application change</param>
        /// <returns>Whether the change was successful</returns>
        public bool SaveChanges(DatabaseContext _db, Application appResponse)
        {
            try
            {
                // Save changes in the database
                _db.SaveChanges();

                return true;
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException)
            {
                // Catch error
                // Detach api key attempted to be changed from the db context - rollback
                _db.Entry(appResponse).State = System.Data.Entity.EntityState.Detached;

                // Error
                return false;
            }
        }


        /// <summary>
        /// Creates an email to send an api key to newly registered applications.
        /// </summary>
        /// <param name="receiverEmail"></param>
        /// <param name="apiKey"></param>
        /// <returns>Whether email was successfully sent</returns>
        public bool SendAppRegistrationApiKeyEmail(string receiverEmail, string apiKey)
        {
            try
            {
                string registrationSubjectString = "KFC SSO Registration";
                string userFullName = receiverEmail;
                string template = "Hi, \r\n" +
                                                 "You recently registered your application to the KFC SSO portal.\r\n" +
                                                 "Below is a single-use API Key to publish your application into the portal.\r\n {0}" +
                                                 "If you did not register to KFC, please contact us by responding to this email.\r\n\r\n" +
                                                 "Thanks, KFC Team";
                string data = apiKey;
                string resetPasswordBodyString = string.Format(template, data);

                //Create the message that will be sent
                MimeMessage emailToSend = _emailService.createEmailPlainBody(userFullName, receiverEmail, registrationSubjectString, resetPasswordBodyString);
                //Send the email with the message
                _emailService.sendEmail(emailToSend);
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        /// <summary>
        /// Creates an email to send new api keys to applications.
        /// </summary>
        /// <param name="receiverEmail"></param>
        /// <param name="apiKey"></param>
        /// <returns>Whether email was successfully sent.</returns>
        public bool SendNewApiKeyEmail(string receiverEmail, string apiKey)
        {
            try
            {
                string newKeySubjectString = "KFC SSO New API Key";
                string userFullName = receiverEmail;
                string template = "Hi, \r\n" +
                                                 "You recently requested a new API Key for you KFC application.\r\n" +
                                                 "Below is a new single-use API Key to publish your application into the portal.\r\n {0}" +
                                                 "If you did not make this request, please contact us by responding to this email.\r\n\r\n" +
                                                 "Thanks, KFC Team";
                string data = apiKey;
                string resetPasswordBodyString = string.Format(template, data);

                //Create the message that will be sent
                MimeMessage emailToSend = _emailService.createEmailPlainBody(userFullName, receiverEmail, newKeySubjectString, resetPasswordBodyString);
                //Send the email with the message
                _emailService.sendEmail(emailToSend);

                return true;
            }
            catch(Exception)
            {
                return false;
            }

        }
    }
}
