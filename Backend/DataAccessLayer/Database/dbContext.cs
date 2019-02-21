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
        public DatabaseContext()
        {
            this.Database.Connection.ConnectionString = "Data Source=localhost;Initial Catalog=SSO;Integrated Security=True";
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Session> Sessions { get; set; }


    }
}
