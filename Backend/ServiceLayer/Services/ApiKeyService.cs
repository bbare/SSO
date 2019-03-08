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

        // Token Services instance
        private ITokenService _tokenService;

        public ApiKeyService()
        {
            _ApiKeyRepo = new ApiKeyRepository();
            _tokenService = new TokenService();
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
                // Generate a unique key
                key.Key = _tokenService.GenerateToken();
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
        public ApiKey DeleteKey(DatabaseContext _db, Guid id)
        {
            return _ApiKeyRepo.DeleteKey(_db, id);
        }

        /// <summary>
        /// Call the Api Key repository to retrieve an api key record by id field
        /// </summary>
        /// <param name="_db">database</param>
        /// <param name="id">api key id</param>
        /// <returns>The retrieved api key</returns>
        public ApiKey GetKey(DatabaseContext _db, Guid id)
        {
            return _ApiKeyRepo.GetKey(_db, id);
        }

        /// <summary>
        /// Call the Api Key repository to retriev an api key record by key field
        /// </summary>
        /// <param name="_db">database</param>
        /// <param name="key">key value of api key</param>
        /// <returns></returns>
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
        
    }
}
