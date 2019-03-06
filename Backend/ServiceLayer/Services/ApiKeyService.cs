using System;
using System.Web.Security;
using DataAccessLayer.Database;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories;
using System.Net.Mail;

namespace ServiceLayer.Services
{
    public class ApiKeyService : IApiKeyService
    {
        // Api Key Repository instance
        private ApiKeyRepository _ApiKeyRepo;

        public ApiKeyService()
        {
            _ApiKeyRepo = new ApiKeyRepository();
        }

        /// <summary>
        /// Call the Api Key repository to create a new api key record
        /// </summary>
        /// <param name="_db">database</param>
        /// <param name="key">api key</param>
        /// <returns>The created api key</returns>
        public ApiKey CreateKey(DatabaseContext _db, ApiKey key)
        {
            // Keep generating a key until it is unique
            bool existing = true;
            while (existing)
            {
                // Generate a key
                key.Key = GenerateKey();
                // Check if the key exists in the database
                if(!_ApiKeyRepo.IsExistingKey(_db, key))
                {
                    existing = false;
                }
            }
            // Create a new api key
            // TODO: Encrypt the ApiKey.key before storing
            return _ApiKeyRepo.CreateNewKey(_db, key);
        }

        /// <summary>
        /// Call the Api Key repository to delete an api key record
        /// </summary>
        /// <param name="_db">database</param>
        /// <param name="key">key value of api key</param>
        /// <returns>The deleted api key</returns>
        public ApiKey DeleteKey(DatabaseContext _db, string key)
        {
            return _ApiKeyRepo.DeleteKey(_db, key);
        }

        /// <summary>
        /// Generate a unique key.
        /// </summary>
        /// <returns>The key</returns>
        public string GenerateKey()
        {
            // TODO: generate a more secure password.
            return Membership.GeneratePassword(50,0);
        }

        /// <summary>
        /// Call the Api Key repository to retrieve an api key record
        /// </summary>
        /// <param name="_db">database</param>
        /// <param name="key">key value of api key</param>
        /// <returns>The retrieved api key</returns>
        public ApiKey GetKey(DatabaseContext _db, string key)
        {
            return _ApiKeyRepo.GetKey(_db, key);
        }

        /// <summary>
        /// Call the Api Key repository to update an api key record
        /// </summary>
        /// <param name="_db">database</param>
        /// <param name="key">api key</param>
        /// <returns>The updated api key</returns>
        public ApiKey UpdateKey(DatabaseContext _db, ApiKey key)
        {
            return _ApiKeyRepo.UpdateKey(_db, key);
        }

        /// <summary>
        /// Email api key to email attached to individual application
        /// </summary>
        /// <param name="appEmail">application email</param>
        /// <param name="apiKey">api key</param>
        public void EmailKey(string appEmail, string apiKey)
        {
            // TODO: Make this work
            MailMessage objeto_mail = new MailMessage();
            SmtpClient client = new SmtpClient();
            client.Port = 25;
            client.Host = "smtp.internal.mycompany.com";
            client.Timeout = 10000;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential("user", "Password");
            objeto_mail.From = new MailAddress("from@server.com");
            objeto_mail.To.Add(new MailAddress("to@server.com"));
            objeto_mail.Subject = "Password Recover";
            objeto_mail.Body = "Message";
            client.Send(objeto_mail);
        }
    }
}
