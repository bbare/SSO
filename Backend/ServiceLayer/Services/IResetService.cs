using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Database;
using DataAccessLayer.Models;

namespace ServiceLayer.Services
{
    public interface IResetService
    {
        PasswordReset CreatePasswordReset(DatabaseContext _db, PasswordReset passwordResetID);
        PasswordReset GetPasswordReset(DatabaseContext _db, string email);
        PasswordReset UpdatePasswordReset(DatabaseContext _db, PasswordReset passwordResetID);
        PasswordReset DeletePasswordReset(DatabaseContext _db, string resetID);
        bool ExistingReset(DatabaseContext _db, string resetID);
    }
}
