using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;
using System.Data.SqlClient;

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
            //dev
            this.Database.Connection.ConnectionString = "Data Source=localdb;Initial Catalog=SSO;Integrated Security=True";

            //release
            //this.Database.Connection.ConnectionString = "Data Source=" + hostname + ";Initial Catalog=" + dbname + ";User ID=" + username + ";Password=" + password + ";";
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Application> Applications { get; set; }
        public DbSet<ApiKey> Keys { get; set; }
        public DbSet<PasswordReset> ResetIDs { get; set; }
    }
}