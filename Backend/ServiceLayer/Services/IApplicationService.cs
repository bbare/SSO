using DataAccessLayer.Database;
using DataAccessLayer.Models;
using System;

namespace ServiceLayer.Services
{
    public interface IApplicationService
    {
        // CRUD
        Application CreateApplication(DatabaseContext _db, Application app);
        Application GetApplication(DatabaseContext _db, string url);
        Application DeleteApplication(DatabaseContext _db, string url);
        Application UpdateApplication(DatabaseContext _db, Application app);
    }
}
