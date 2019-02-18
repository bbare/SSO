using DataAccessLayer.Models;
using DataAccessLayer.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class SessionReposity
    {
        public bool ValidateSession(Guid Id)
        {
            using (var _db = new DatabaseContext())
            {
                Session session = _db.Sessions
                    .Where(s => s.UserId == Id && s.ExpiresAt < DateTime.UtcNow)
                    .FirstOrDefault();

                if (session == null)
                    return false;
                return true;
            }
        }
    }
}
