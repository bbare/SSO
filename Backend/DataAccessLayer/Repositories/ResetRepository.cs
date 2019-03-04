using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Models;
using DataAccessLayer.Database;

namespace DataAccessLayer.Repositories
{
    public class ResetRepository
    {
        
        public PasswordReset GetReset(DatabaseContext _db, string resetID)
        {
            var returnedResetID = _db.ResetIDs.Find(resetID);
            return returnedResetID;
        }
        
        public PasswordReset CreateReset(DatabaseContext _db, PasswordReset newPasswordReset)
        {
            _db.ResetIDs.Add(newPasswordReset);
            _db.SaveChanges();
            return newPasswordReset;
        }

        public PasswordReset UpdateReset(DatabaseContext _db, PasswordReset updatedPasswordReset)
        {
            var originalPasswordReset = _db.ResetIDs.Find(updatedPasswordReset.resetID);

            _db.Entry(originalPasswordReset).CurrentValues.SetValues(updatedPasswordReset);
            _db.SaveChanges();
            return updatedPasswordReset;
        }

        public PasswordReset DeleteReset(DatabaseContext _db, string passwordResetIDToDelete)
        {
            var passwordReset = _db.ResetIDs.Find(passwordResetIDToDelete);
            _db.ResetIDs.Remove(passwordReset);
            _db.SaveChanges();
            return passwordReset;
        }

        public bool ExistingReset(DatabaseContext _db, string resetID)
        {
            var result = GetReset(_db, resetID);
            if (result != null)
            {
                return true;
            }
            return false;
        }
    }
}
