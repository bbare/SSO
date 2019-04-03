using System.Linq;
using DataAccessLayer.Models;
using DataAccessLayer.Database;
using System.Data.Entity;

namespace DataAccessLayer.Repositories
{
    public class ResetRepository
    {
        
        public PasswordReset GetReset(DatabaseContext _db, string resetToken)
        {
            var returnedResetToken = _db.PasswordResets
                                     .Where(r => r.ResetToken == resetToken)
                                     .FirstOrDefault<PasswordReset>();
            return returnedResetToken;
        }
        
        public PasswordReset CreateReset(DatabaseContext _db, PasswordReset newPasswordReset)
        {
            _db.Entry(newPasswordReset).State = EntityState.Added;
            return newPasswordReset;
        }

        public PasswordReset UpdateReset(DatabaseContext _db, PasswordReset updatedPasswordReset)
        {
            _db.Entry(updatedPasswordReset).State = EntityState.Modified;
            return updatedPasswordReset;
        }

        public PasswordReset DeleteReset(DatabaseContext _db, string passwordResetTokenToDelete)
        {
            var PasswordResetToRemove = _db.PasswordResets
                                     .Where(r => r.ResetToken == passwordResetTokenToDelete)
                                     .FirstOrDefault<PasswordReset>();

            if (PasswordResetToRemove == null)
                return null;
            _db.Entry(PasswordResetToRemove).State = EntityState.Deleted;
            return PasswordResetToRemove;
        }
    }
}
