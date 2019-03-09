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
        
        public PasswordReset GetReset(DatabaseContext _db, string resetToken)
        {
            var returnedResetToken = _db.ResetIDs.Find(resetToken);
            return returnedResetToken;
        }
        
        public PasswordReset CreateReset(DatabaseContext _db, PasswordReset newPasswordReset)
        {
            _db.ResetIDs.Add(newPasswordReset);
            return newPasswordReset;
        }

        public PasswordReset UpdateReset(DatabaseContext _db, PasswordReset updatedPasswordReset)
        {
            var originalPasswordReset = _db.ResetIDs.Find(updatedPasswordReset.ResetToken);

            _db.Entry(originalPasswordReset).CurrentValues.SetValues(updatedPasswordReset);
            return updatedPasswordReset;
        }

        public PasswordReset DeleteReset(DatabaseContext _db, string passwordResetTokenToDelete)
        {
            var passwordReset = _db.ResetIDs.Find(passwordResetTokenToDelete);
            _db.ResetIDs.Remove(passwordReset);
            return passwordReset;
        }

        public bool ExistingReset(DatabaseContext _db, string resetToken)
        {
            var result = GetReset(_db, resetToken);
            if (result != null)
            {
                return true;
            }
            return false;
        }
    }
}
