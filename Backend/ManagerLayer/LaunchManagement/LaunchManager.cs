using DataAccessLayer.Database;
using DataAccessLayer.Models;
using ServiceLayer.Services;
using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;

namespace ManagerLayer.LaunchManagement
{
    public class LaunchPayload
    {
        Guid ssoUserId { get; set; }
        string email { get; set; }
        DateTime timestamp { get; set; }
        DateTime signature { get; set; }
    }

    public class LaunchManager : ILaunchManager
    {
        public LaunchPayload SignLaunch(DatabaseContext _db, Session session, Guid appId)
        {
            ApplicationService applicationService = new ApplicationService();
            Application app = applicationService.GetApplication(_db, appId);

            if (app == null)
            {
                throw new ArgumentException();
            }

            //var hmacsha1 = new HMACSHA1(app.Title);

            return new LaunchPayload {};
        }
    }
}
