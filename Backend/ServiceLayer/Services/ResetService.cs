using DataAccessLayer.Database;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories;


namespace ServiceLayer.Services
{
    public class ResetService : IResetService
    {
        private ResetRepository _resetRepo;

        public ResetService()
        {
            _resetRepo = new ResetRepository();
        }

        public PasswordReset CreatePasswordReset(DatabaseContext _db, PasswordReset passwordReset)
        {
            return _resetRepo.CreateReset(_db, passwordReset);
        }

        public PasswordReset DeletePasswordReset(DatabaseContext _db, string resetToken)
        {
            return _resetRepo.DeleteReset(_db, resetToken);
        }

        public PasswordReset GetPasswordReset(DatabaseContext _db, string resetToken)
        {
            return _resetRepo.GetReset(_db, resetToken);
        }

        public PasswordReset UpdatePasswordReset(DatabaseContext _db, PasswordReset passwordResetID)
        {
            return _resetRepo.UpdateReset(_db, passwordResetID);
        }

        public bool ExistingReset(DatabaseContext _db, string resetToken)
        {
            var result = GetPasswordReset(_db, resetToken);
            if (result != null)
            {
                return true;
            }
            return false;
        }
    }
}
