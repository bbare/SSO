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
        string username = "Admin";
        string password = "";
        string hostname = "mydbinstance.ce5cmkuh7zii.us-east-2.rds.amazonaws.com";
        string port = "1433";
        string dbname = "mydbinstance";

        public DatabaseContext()
        {
            // set a system enviorment variable for dev, "Data Source=(localdb);Initial Catalog=SSO;Integrated Security = True"
            //var connectionString = Environment.GetEnvironmentVariable("KFC_SSO_DEV_DATABASE", EnvironmentVariableTarget.User);
            //this.Database.Connection.ConnectionString = connectionString;

            this.Database.Connection.ConnectionString = "Data Source =(LocalDB)\\MSSQLLocalDB; AttachDbFilename = C:\\Users\\Vik\\Documents\\Project\\Current SSO LOgout Vicotoakdas Kim\\SSO\\Backend\\DataAccessLayer\\DatabaseContext    .mdf;Integrated Security = True";
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Application> Applications { get; set; }
        public DbSet<ApiKey> Keys { get; set; }
        public DbSet<PasswordReset> ResetIDs { get; set; }
    }
}
