using DataAccessLayer.Database;
using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class ClaimRepository
    {
        public int CreateClaim(Claim claim)
        {
            using (var _db = new DatabaseContext())
            {
                claim.UpdatedAt = DateTime.UtcNow;
                try
                {
                    _db.Claims.Add(claim);
                    return _db.SaveChanges();
                }
                catch(Exception)
                {
                    return 0;
                }
            }
        }
      
        public Service GetService(string claimName)
        {
            using (var _db = new DatabaseContext())
            {
                Service service = _db.Services
                    .Where(c => c.ServiceName == claimName)
                    .FirstOrDefault();
                return service;
            }
        }

        public void AddServiceToUser(Guid userId, Guid serviceId)
        {
            using (var _db = new DatabaseContext())
            {
                var u = new Claim
                {
                    UserId = userId,
                    ServiceId = serviceId
                };
                _db.Claims.Add(u);
                _db.SaveChanges();
            }
        }

        public bool UserHasServiceAccess(Guid userId, Guid serviceId)
        {
            using (var _db = new DatabaseContext())
            {
                int count = _db.Claims
                    .Where(c => c.UserId == userId && c.ServiceId == serviceId)
                    .Count();

                return count > 0;
            }
        }
    }
}
