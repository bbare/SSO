using System;
using System.Collections.Generic;
using DataAccessLayer.Models;
using DataAccessLayer.Database;
using DataAccessLayer.Repositories;

namespace ServiceLayer.Services
{
    interface IOperationService
    {
        int CreateService(DatabaseContext _db, Service service);
        int CreateService(DatabaseContext _db, string serviceName);
        int DisableService(DatabaseContext _db, Guid serviceId);
        int EnableService(DatabaseContext _db, Guid serviceId);
        int DeleteService(DatabaseContext _db, Guid serviceId);
    }
}
