using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;
using System.Data.SqlClient;
using System.Configuration;

namespace DataAccessLayer.Database
{
    public class DatabaseContext : DbContext
    {

        public DatabaseContext()
        {
            // set a system enviorment variable for dev, "Data Source=(localdb);Initial Catalog=SSO;Integrated Security = True"
            //var connectionString = Environment.GetEnvironmentVariable("KFC_SSO_DEV_DATABASE", EnvironmentVariableTarget.User);
            //this.Database.Connection.ConnectionString = connectionString;
            this.Database.Connection.ConnectionString = "Data Source=LAPTOP-ET31PVV2;Initial Catalog=SSO;Integrated Security = True";
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Application> Applications { get; set; }
        public DbSet<ApiKey> Keys { get; set; }
        public DbSet<PasswordReset> PasswordResets { get; set; }
    }
}
