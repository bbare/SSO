using DataAccessLayer.Database;
using DataAccessLayer.Models;

namespace ManagerLayer
{
    public interface IAuthorizationManager
    {
        string GenerateSessionToken();
        Session CreateSession(DatabaseContext _db, User user);
        Session ValidateAndUpdateSession(DatabaseContext _db, string token);
        Session DeleteSession(DatabaseContext _db, string token);
    }
}
