using DataAccessLayer.Database;
using DataAccessLayer.Models;
using System;

namespace ServiceLayer.Services
{
    public interface IApiKeyService
    {
        // CRUD
        ApiKey CreateKey(DatabaseContext _db, ApiKey key);
        ApiKey GetKey(DatabaseContext _db, string key);
        ApiKey DeleteKey(DatabaseContext _db, string key);
        ApiKey UpdateKey(DatabaseContext _db, ApiKey key);

        /// <summary>
        /// Generate an Api Key for individual applications to publish themselves.
        /// </summary>
        /// <returns>Api Key</returns>
        string GenerateKey();

        /// <summary>
        /// Email an Api Key to the email attached to the individual application
        /// </summary>
        /// <param name="appEmail">Application email</param>
        /// <param name="apiKey">Api Key</param>
        void EmailKey(string appEmail, string apiKey);
    }
}
