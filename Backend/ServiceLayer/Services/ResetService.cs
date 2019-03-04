using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Database;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories;
using MimeKit;
using MailKit;
using MailKit.Net.Smtp;


namespace ServiceLayer.Services
{
    public class ResetService: IResetService
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

        public PasswordReset DeletePasswordReset(DatabaseContext _db, string resetID)
        {
            return _resetRepo.DeleteReset(_db, resetID);
            
        }

        public PasswordReset GetPasswordReset(DatabaseContext _db, string resetID)
        {
            return _resetRepo.GetReset(_db, resetID);
        }

        public PasswordReset UpdatePasswordReset(DatabaseContext _db, PasswordReset passwordResetID)
        {
            return _resetRepo.UpdateReset(_db, passwordResetID);
        }

        public bool ExistingReset(DatabaseContext _db, string resetID)
        {
            return _resetRepo.ExistingReset(_db, resetID);
        }
    }
}
