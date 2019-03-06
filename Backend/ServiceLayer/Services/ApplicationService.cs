using System;
using DataAccessLayer.Database;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories;


namespace ServiceLayer.Services
{
    public class ApplicationService : IApplicationService
    {
        // Application repository instance
        private ApplicationRepository _ApplicationRepo;

        public ApplicationService()
        {
            _ApplicationRepo = new ApplicationRepository();
        }

        /// <summary>
        /// Call the application repository to create a new application record
        /// </summary>
        /// <param name="_db">database</param>
        /// <param name="app">application</param>
        /// <returns>The application created</returns>
        public Application CreateApplication(DatabaseContext _db, Application app)
        {
            // The application already exists in the database
            if (_ApplicationRepo.IsExistingApplication(_db, app))
            {
                return null;
            }

            // Create a new application.
            return _ApplicationRepo.CreateNewApplication(_db, app);
        }

        /// <summary>
        /// Call the application repository to delete an application record
        /// </summary>
        /// <param name="_db">database</param>
        /// <param name="url">application url</param>
        /// <returns>The deleted application</returns>
        public Application DeleteApplication(DatabaseContext _db, string url)
        {
            return _ApplicationRepo.DeleteApplication(_db, url);
        }

        /// <summary>
        /// Call the application repository to retrieve an application record
        /// </summary>
        /// <param name="_db">database</param>
        /// <param name="url">application</param>
        /// <returns>The retrieved application</returns>
        public Application GetApplication(DatabaseContext _db, string url)
        {
            return _ApplicationRepo.GetApplication(_db, url);
        }

        /// <summary>
        /// Call the application repository to update an application record
        /// </summary>
        /// <param name="_db">database</param>
        /// <param name="app">application</param>
        /// <returns>The updated application</returns>
        public Application UpdateApplication(DatabaseContext _db, Application app)
        {
            return _ApplicationRepo.UpdateApplication(_db, app);
        }
    }
}
