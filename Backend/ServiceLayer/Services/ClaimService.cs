using DataAccessLayer.Database;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Services
{
    public class ClaimService : IClaimService
    {
        private ClaimRepository _ClaimRepo;

        public ClaimService()
        {
            _ClaimRepo = new ClaimRepository();
        }

        public int CreateClaim(Claim claim)
        {
            return _ClaimRepo.CreateClaim(claim);
        }

        public int CreateClaim(Guid userId, Guid serviceId)
        {
            Claim claim = new Claim
            {
                UserId = userId,
                ServiceId = serviceId,
                UpdatedAt = DateTime.UtcNow
            };
            return _ClaimRepo.CreateClaim(claim);
        }

        public Service GetService(string claimName)
        {
            return _ClaimRepo.GetService(claimName);
        }

        public void AddServiceToUser(User user, Service service)
        {
            _ClaimRepo.AddServiceToUser(user.Id, service.Id);
        }

        public bool UserHasServiceAccess(User user, Service service)
        {
            if (service.Disabled) return false;
            return _ClaimRepo.UserHasServiceAccess(user.Id, service.Id);
        }
    }
}
