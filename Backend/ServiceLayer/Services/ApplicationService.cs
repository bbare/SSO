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
        public Application DeleteApplication(DatabaseContext _db, Guid id)
        {
            return _ApplicationRepo.DeleteApplication(_db, id);
        }

        /// <summary>
        /// Call the application repository to retrieve an application record by id
        /// </summary>
        /// <param name="_db">database</param>
        /// <param name="url">application</param>
        /// <returns>The retrieved application</returns>
        public Application GetApplication(DatabaseContext _db, Guid id)
        {
            return _ApplicationRepo.GetApplication(_db, id);
        }

        /// <summary>
        /// Call the application repository to retrieve an application record by title and email
        /// </summary>
        /// <param name="_db">database</param>
        /// <param name="title">application title</param>
        /// <param name="email">email</param>
        /// <returns></returns>
        public Application GetApplication(DatabaseContext _db, string title, string email)
        {
            return _ApplicationRepo.GetApplication(_db, title, email);
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
