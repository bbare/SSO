using DataAccessLayer.Database;
using DataAccessLayer.Models;
using System;

namespace ManagerLayer.LaunchManagement
{
    public interface ILaunchManager
    {
        LaunchResponse SignLaunch(DatabaseContext _db, Session session, Guid appId);
    }
}
