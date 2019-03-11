using DataAccessLayer.Database;
using DataAccessLayer.Models;
using System;

namespace ServiceLayer.Services
{
    public interface IApplicationService
    {
        // CRUD
        Application CreateApplication(DatabaseContext _db, Application app);
        Application GetApplication(DatabaseContext _db, Guid id);
        Application GetApplication(DatabaseContext _db, string title, string email);
        Application DeleteApplication(DatabaseContext _db, Guid id);
        Application UpdateApplication(DatabaseContext _db, Application app);
    }
}
