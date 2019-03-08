using DataAccessLayer.Database;
using DataAccessLayer.Models;
using System;

namespace ServiceLayer.Services
{
    public interface IApiKeyService
    {
        // CRUD
        ApiKey CreateKey(DatabaseContext _db, ApiKey key);
        ApiKey GetKey(DatabaseContext _db, Guid id);
        ApiKey GetKey(DatabaseContext _db, string key);
        ApiKey DeleteKey(DatabaseContext _db, Guid id);
        ApiKey UpdateKey(DatabaseContext _db, ApiKey key);

        /// <summary>
        /// Generate an Api Key for individual applications to publish themselves.
        /// </summary>
        /// <returns>Api Key</returns>
        string GenerateKey();
    }
}
