using System;
using DataAccessLayer.Database;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories;


namespace ServiceLayer.Services
{
    public static class ApplicationService
    {

        /// <summary>
        /// Call the application repository to create a new application record
        /// </summary>
        /// <param name="_db">database</param>
        /// <param name="app">application</param>
        /// <returns>The application created</returns>
        public static Application CreateApplication(DatabaseContext _db, Application app)
        {
            return ApplicationRepository.CreateNewApplication(_db, app);
        }

        /// <summary>
        /// Call the application repository to delete an application record
        /// </summary>
        /// <param name="_db">database</param>
        /// <param name="url">application url</param>
        /// <returns>The deleted application</returns>
        public static Application DeleteApplication(DatabaseContext _db, Guid id)
        {
            return ApplicationRepository.DeleteApplication(_db, id);
        }

        /// <summary>
        /// Call the application repository to retrieve an application record by id
        /// </summary>
        /// <param name="_db">database</param>
        /// <param name="url">application</param>
        /// <returns>The retrieved application</returns>
        public static Application GetApplication(DatabaseContext _db, Guid id)
        {
            return ApplicationRepository.GetApplication(_db, id);
        }

        /// <summary>
        /// Call the application repository to retrieve an application record by title and email
        /// </summary>
        /// <param name="_db">database</param>
        /// <param name="title">application title</param>
        /// <param name="email">email</param>
        /// <returns></returns>
        public static Application GetApplication(DatabaseContext _db, string title, string email)
        {
            return ApplicationRepository.GetApplication(_db, title, email);
        }

        /// <summary>
        /// Call the application repository to update an application record
        /// </summary>
        /// <param name="_db">database</param>
        /// <param name="app">application</param>
        /// <returns>The updated application</returns>
        public static Application UpdateApplication(DatabaseContext _db, Application app)
        {
            return ApplicationRepository.UpdateApplication(_db, app);
        }
    }
}
