using System;
using DataAccessLayer.Database;
using DataAccessLayer.Models;
using System.Linq;
using System.Data.Entity;

namespace DataAccessLayer.Repositories
{
    public static class ApiKeyRepository
    {
        /// <summary>
        /// Create a new Api Key record
        /// </summary>
        /// <param name="_db">database</param>
        /// <param name="key">api key</param>
        /// <returns>created api key</returns>
        public static ApiKey CreateNewKey(DatabaseContext _db, ApiKey key)
        {
            try
            {
                var apiKey = GetKey(_db, key.Key);
                if (apiKey != null)
                {
                    return null;
                }
                _db.Entry(key).State = EntityState.Added;
                return key;
            }
            catch (Exception)
            {
                return null;
            }
            
        }

        /// <summary>
        /// Delete an Api Key record
        /// </summary>
        /// <param name="_db">database</param>
        /// <param name="id">api key id</param>
        /// <returns>deleted api key</returns>
        public static ApiKey DeleteKey(DatabaseContext _db, Guid id)
        {

            try
            {
                var apiKey = GetKey(_db, id);
                if (apiKey == null)
                {
                    return null;
                }
                _db.Entry(apiKey).State = EntityState.Deleted;
                return apiKey;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Retrieve an Api Key record by id field
        /// </summary>
        /// <param name="_db">database</param>
        /// <param name="id">api key id</param>
        /// <returns>The retrieved api key</returns>
        public static ApiKey GetKey(DatabaseContext _db, Guid id)
        {
            try
            {
                var response = _db.Keys.Find(id);
                return response;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Retrieve an Api Key record by application id and isUsed
        /// </summary>
        /// <param name="_db">database</param>
        /// <param name="appId">application id</param>
        /// <param name="isUsed">whether the key has been used or not</param>
        /// <returns></returns>
        public static ApiKey GetKey(DatabaseContext _db, Guid applicationId, bool isUsed)
        {

            try
            {
                var apiKey = _db.Keys
                .Where(k => k.ApplicationId == applicationId && k.IsUsed == false)
                .FirstOrDefault<ApiKey>();
                return apiKey;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Retrieve an Api Key record by key field
        /// </summary>
        /// <param name="_db">database</param>
        /// <param name="key">key value of api key</param>
        /// <returns></returns>
        public static ApiKey GetKey(DatabaseContext _db, string key)
        {

            try
            {
                var apiKey = _db.Keys
                .Where(k => k.Key == key)
                .FirstOrDefault<ApiKey>();
                return apiKey;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Update an Api Key record
        /// </summary>
        /// <param name="_db">database</param>
        /// <param name="key">api key</param>
        /// <returns></returns>
        public static ApiKey UpdateKey(DatabaseContext _db, ApiKey key)
        {

            try
            {
                var result = GetKey(_db, key.Id);
                if (result == null)
                {
                    return null;
                }
                _db.Entry(key).State = EntityState.Modified;
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

    }
}
