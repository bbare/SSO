using System;
using DataAccessLayer.Database;
using DataAccessLayer.Models;
using System.Linq;
using System.Data.Entity;

namespace DataAccessLayer.Repositories
{
    public class ApplicationRepository
    {
        /// <summary>
        /// Create a new application record
        /// </summary>
        /// <param name="_db">database</param>
        /// <param name="app">application</param>
        /// <returns>Created application</returns>
        public Application CreateNewApplication(DatabaseContext _db, Application app)
        {
            _db.Entry(app).State = EntityState.Added;
            return app;
        }

        /// <summary>
        /// Delete an application record
        /// </summary>
        /// <param name="_db">database</param>
        /// <param name="url">application url</param>
        /// <returns>The deleted application record</returns>
        public Application DeleteApplication(DatabaseContext _db, string url)
        {
            var app = GetApplication(_db, url);
            if (app == null)
            {
                return null;
            }
            _db.Entry(app).State = EntityState.Deleted;
            return app;
        }

        /// <summary>
        /// Retrieve an application record by url field
        /// </summary>
        /// <param name="_db">database</param>
        /// <param name="url">application url</param>
        /// <returns>The retrieved application</returns>
        public Application GetApplication(DatabaseContext _db, string url)
        {
            var app = _db.Applications
                .Where(c => c.LaunchUrl.Equals(url))
                .FirstOrDefault<Application>();
            return app;
        }

        /// <summary>
        /// Retrieve an application record by id field
        /// </summary>
        /// <param name="_db">database</param>
        /// <param name="id">application id</param>
        /// <returns>The retrieved application</returns>
        public Application GetApplication(DatabaseContext _db, Guid id)
        {
            return _db.Applications.Find(id);
        }

        /// <summary>
        /// Update an application record
        /// </summary>
        /// <param name="_db">database</param>
        /// <param name="app">application</param>
        /// <returns>The updated application</returns>
        public Application UpdateApplication(DatabaseContext _db, Application app)
        {
            _db.Entry(app).State = EntityState.Modified;
            return app;
        }

        /// <summary>
        /// Checks if an application record exists in the database.
        /// </summary>
        /// <param name="_db">database</param>
        /// <param name="app">application</param>
        /// <returns>Whether the application exists</returns>
        public bool IsExistingApplication(DatabaseContext _db, Application app)
        {
            // Retrieve the application
            var result = GetApplication(_db, app.LaunchUrl);
            if (result != null) // Application exists
            {
                return true;
            }
            // Application does not exists
            return false;
        }

        /// <summary>
        /// Checks if an application record exists in the database.
        /// </summary>
        /// <param name="_db">database</param>
        /// <param name="url">application url</param>
        /// <returns>Whether the application exists</returns>
        public bool IsExistingApplication(DatabaseContext _db, string url)
        {
            // Retrieve the application
            var result = GetApplication(_db, url);
            if (result != null) // Application exists
            {
                return true;
            }
            // Application does not exists
            return false;
        }
    }
}
