using DataAccessLayer.Database;
using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Services
{
    public interface IUserService
    {
        // CRUD
        User CreateUser(DatabaseContext _db, User user);
        User GetUser(DatabaseContext _db, string email);
        User GetUser(DatabaseContext _db, Guid Id);
        User DeleteUser(DatabaseContext _db, Guid Id);
        User UpdateUser(DatabaseContext _db, User user);
        bool IsManagerOver(DatabaseContext _db, User user, User subject);
    }
}
