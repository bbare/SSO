using System;
using System.Collections.Generic;
using DataAccessLayer.Models;
using DataAccessLayer.Database;
using DataAccessLayer.Repositories;

namespace ServiceLayer.Services
{
    public class OperationService : IOperationService
    {
        private readonly OperationRepository _OperationRepo;

        public OperationService()
        {
            _OperationRepo = new OperationRepository();
        }

        public int CreateService(DatabaseContext _db, Service service)
        {
            return _OperationRepo.CreateService(_db, service);
        }

        public int CreateService(DatabaseContext _db, string serviceName)
        {
            return _OperationRepo.CreateService(_db, serviceName);
        }

        public int DisableService(DatabaseContext _db, Guid serviceId)
        {
            return _OperationRepo.DisableService(_db, serviceId);
        }

        public int EnableService(DatabaseContext _db, Guid serviceId)
        {
            return _OperationRepo.EnableService(_db, serviceId);
        }
        
        public int DeleteService(DatabaseContext _db, Guid serviceId)
        {
            return _OperationRepo.DeleteService(_db, serviceId);
        }

        public bool IsServiceDisabled(DatabaseContext _db, Guid serviceId)
        {
            return _OperationRepo.IsServiceDisabled(_db, serviceId);
        }
    }
}
