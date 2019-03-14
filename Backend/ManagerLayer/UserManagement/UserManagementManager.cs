// --------------------
// THIS MANAGER IS BEING DEPRECATED. PLEASE DO NOT ADD ANY NEW METHODS OR USE THE METHODS WITHIN.
// PLEASE USE USERMANAGER.CS
// --------------------

using DataAccessLayer.Database;
using DataAccessLayer.Models;
using ServiceLayer.Services;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagerLayer.UserManagement
{
    public class UserManagementManager
    {
        private IPasswordService _passwordService;
        private IUserService _userService;

        public UserManagementManager()
        {
            _userService = new UserService();
        }

        private DatabaseContext CreateDbContext()
        {
            return new DatabaseContext();
        }

        public int DeleteUser(User user)
        {
            using (var _db = CreateDbContext())
            {
                var response = _userService.DeleteUser(_db, user.Id);
                // will return null if user does not exist
                return _db.SaveChanges();
            }
        }

        public int DeleteUser(Guid id)
        {
            using (var _db = CreateDbContext())
            {
                var response = _userService.DeleteUser(_db, id);
                return _db.SaveChanges();
            }
        }

        public User GetUser(Guid id)
        {
            using (var _db = CreateDbContext())
            {
                return _userService.GetUser(_db, id);
            }
        }

        public User GetUser(string email)
        {
            using (var _db = CreateDbContext())
            {
                return  _userService.GetUser(_db, email);
            }
        }

        public int DisableUser(User user)
        {
            return ToggleUser(user, false);
        }

        public int EnableUser(User user)
        {
            return ToggleUser(user, true);
        }

        public int ToggleUser(User user, bool? enable)
        {
            using (var _db = CreateDbContext())
            {
                if (enable == null) enable = !user.Disabled;
                user.Disabled = !(bool)enable;
                var response = _userService.UpdateUser(_db, user);
                return _db.SaveChanges();
            }
        }

        public int UpdateUser(User user)
        {
            using (var _db = CreateDbContext())
            {
                var response = _userService.UpdateUser(_db, user);
                try
                {
                    return _db.SaveChanges();
                }
                catch (DbEntityValidationException)
                {
                    // catch error
                    // rollback changes
                    _db.Entry(response).CurrentValues.SetValues(_db.Entry(response).OriginalValues);
                    _db.Entry(response).State = System.Data.Entity.EntityState.Unchanged;
                    return 0;
                }
            }
        }

        public bool ExistingUser(string email)
        {
            using (var _db = CreateDbContext())
            {
                return _userService.ExistingUser(_db, email);
            }
        }
    }
}
