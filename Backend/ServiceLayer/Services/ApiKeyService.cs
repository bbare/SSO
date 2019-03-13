using System;
using System.Web.Security;
using DataAccessLayer.Database;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories;
using System.Net.Mail;

namespace ServiceLayer.Services
{
    public static class ApiKeyService
    {

        /// <summary>
        /// Call the Api Key repository to create a new api key record
        /// </summary>
        /// <param name="_db">database</param>
        /// <param name="key">api key</param>
        /// <returns>The created api key</returns>
        public static ApiKey CreateKey(DatabaseContext _db, ApiKey key)
        {
            return ApiKeyRepository.CreateNewKey(_db, key);
        }

        /// <summary>
        /// Call the Api Key repository to delete an api key record
        /// </summary>
        /// <param name="_db">database</param>
        /// <param name="key">key value of api key</param>
        /// <returns>The deleted api key</returns>
        public static ApiKey DeleteKey(DatabaseContext _db, Guid id)
        {
            return ApiKeyRepository.DeleteKey(_db, id);
        }

        /// <summary>
        /// Call the Api Key repository to retrieve an api key record by id field
        /// </summary>
        /// <param name="_db">database</param>
        /// <param name="id">api key id</param>
        /// <returns>The retrieved api key</returns>
        public static ApiKey GetKey(DatabaseContext _db, Guid id)
        {
            return ApiKeyRepository.GetKey(_db, id);
        }

        /// <summary>
        /// Call the Api Key repository to retrieve an api key record by application id and isUsed
        /// </summary>
        /// <param name="_db">database</param>
        /// <param name="applicationId"></param>
        /// <param name="isUsed">whether the key has been used</param>
        /// <returns></returns>
        public static ApiKey GetKey(DatabaseContext _db, Guid applicationId, bool isUsed)
        {
            return ApiKeyRepository.GetKey(_db, applicationId, isUsed);
        }

        /// <summary>
        /// Call the Api Key repository to retriev an api key record by key field
        /// </summary>
        /// <param name="_db">database</param>
        /// <param name="key">key value of api key</param>
        /// <returns></returns>
        public static ApiKey GetKey(DatabaseContext _db, string key)
        {
            return ApiKeyRepository.GetKey(_db, key);
        }

        /// <summary>
        /// Call the Api Key repository to update an api key record
        /// </summary>
        /// <param name="_db">database</param>
        /// <param name="key">api key</param>
        /// <returns>The updated api key</returns>
        public static ApiKey UpdateKey(DatabaseContext _db, ApiKey key)
        {
            return ApiKeyRepository.UpdateKey(_db, key);
        }
        
    }
}
