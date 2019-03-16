using DataAccessLayer.Database;
using DataAccessLayer.Models;
using System;

namespace ManagerLayer.LaunchManagement
{
    public interface ILaunchManager
    {
        LaunchPayload SignLaunch(DatabaseContext _db, Session session, Guid appId);
    }
}
