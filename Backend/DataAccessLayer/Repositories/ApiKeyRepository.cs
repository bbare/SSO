using System;
using DataAccessLayer.Database;
using DataAccessLayer.Models;
using System.Linq;
using System.Data.Entity;

namespace DataAccessLayer.Repositories
{
    public class ApiKeyRepository
    {
        /// <summary>
        /// Create a new Api Key record
        /// </summary>
        /// <param name="_db">database</param>
        /// <param name="key">api key</param>
        /// <returns>created api key</returns>
        public ApiKey CreateNewKey(DatabaseContext _db, ApiKey key)
        {
            _db.Entry(key).State = EntityState.Added;
            return key;
        }

        /// <summary>
        /// Delete an Api Key record
        /// </summary>
        /// <param name="_db">database</param>
        /// <param name="key">key value of api key</param>
        /// <returns>deleted api key</returns>
        public ApiKey DeleteKey(DatabaseContext _db, string key)
        {
            var apiKey = _db.Keys
                .Where(c => c.Key.Equals(key))
                .FirstOrDefault<ApiKey>();
            if(apiKey == null)
            {
                return null;
            }
            _db.Entry(apiKey).State = EntityState.Deleted;
            return apiKey;
        }

        /// <summary>
        /// Retrieve an Api Key record
        /// </summary>
        /// <param name="_db">database</param>
        /// <param name="key">key value of api key</param>
        /// <returns>The retrieved api key</returns>
        public ApiKey GetKey(DatabaseContext _db, string key)
        {
            return _db.Keys.Find(key);
        }

        /// <summary>
        /// Update an Api Key record
        /// </summary>
        /// <param name="_db">database</param>
        /// <param name="key">api key</param>
        /// <returns></returns>
        public ApiKey UpdateKey(DatabaseContext _db, ApiKey key)
        {
            _db.Entry(key).State = EntityState.Modified;
            return key;
        }

        /// <summary>
        /// Checks if an Api Key record exists in the database
        /// </summary>
        /// <param name="_db">database</param>
        /// <param name="key">api key</param>
        /// <returns></returns>
        public bool IsExistingKey(DatabaseContext _db, ApiKey key)
        {
            // Retrieve the api key
            var result = GetKey(_db, key.Key);
            if(result != null) // The api key exists
            {
                return true;
            }
            // The api key does not exist
            return false;
        }
    }
}
