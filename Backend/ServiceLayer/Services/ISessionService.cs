using DataAccessLayer.Models;
using DataAccessLayer.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceLayer.Services
{
    public interface ISessionService
    {
        Session CreateSession(DatabaseContext _db, Session session);
        Session ValidateSession(DatabaseContext _db, string token, Guid userId);
        Session UpdateSession(DatabaseContext _db, Session session);
        Session DeleteSession(DatabaseContext _db, string  token, Guid userId);
    }
}
