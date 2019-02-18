using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Models;
using System.Data.Entity;
using DataAccessLayer.Database;

namespace DataAccessLayer.Repositories
{
    public class OperationRepository
    {

        public int CreateService(DatabaseContext _db, Service service)
        {
            try
            {
                if (service == null || service.Id == null || service.ServiceName == null)
                    return -1;
                _db.Services.Add(service);
                return 1;

            } 
            catch(Exception)
            {
                return 0;
            }
        }

        public int CreateService(DatabaseContext _db, string serviceName)
        {
            Service service = new Service
            {
                ServiceName = serviceName,
                Disabled = false,
                UpdatedAt = DateTime.UtcNow
            };

            return CreateService(_db, service);
        }

        public int DeleteService(DatabaseContext _db, Guid id)
        {
            try
            {
                Service service = GetService(_db, id);
                if (service == null || service.Id == null || service.ServiceName == null)
                    return -1;
                service.Disabled = true;
                service.UpdatedAt = DateTime.UtcNow;
                _db.Entry(service).State = EntityState.Deleted;
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public int EnableService(DatabaseContext _db, Guid id)
        {
            return ToggleService(_db, id, false);
        }

        public int DisableService(DatabaseContext _db, Guid id)
        {
            return ToggleService(_db, id, true);
        }

        private int ToggleService(DatabaseContext _db, Guid id, bool state)
        {
            try
            {
                Service service = GetService(_db, id);
                if (service == null)
                    return -1;
                service.Disabled = state;
                service.UpdatedAt = DateTime.UtcNow;
                _db.Entry(service).State = EntityState.Modified;
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        private Service GetService(DatabaseContext _db, Guid id)
        {
            Service service = _db.Services
                    .Where(s => s.Id == id)
                    .FirstOrDefault();
            return service;
        }

        public bool IsServiceDisabled(DatabaseContext _db, Guid guid)
        {
            var service = GetService(_db, guid);
            if (service == null)
                return true; // if DNE, then service IS disabled
            return service.Disabled;
        }
    }
}
