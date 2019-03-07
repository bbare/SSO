using DataAccessLayer.Database;
using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class UserRepository
    {

        public bool ValidatePassword(User user, string passwordSubmittedHash)
        {
            using (var _db = new DatabaseContext())
            {
                var storedHash = _db.Users.Find(user.Id).PasswordHash;
                if (storedHash == passwordSubmittedHash)
                {
                    return true;
                }
                return false;
            }
        }
    }
}
