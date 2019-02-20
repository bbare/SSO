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
            this.Database.Connection.ConnectionString = "Data Source=DESKTOP-9OTEOUK\\MSSQLSERVER01;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Session> Sessions { get; set; }


    }
}
