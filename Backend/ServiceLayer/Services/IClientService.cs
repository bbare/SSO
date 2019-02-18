using DataAccessLayer.Database;
using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Services
{
    public interface IClientService
    {
        Client CreateClient(DatabaseContext _db, Client client);
        Client DeleteClient(DatabaseContext _db, Guid Id);
        Client GetClient(DatabaseContext _db, Guid Id);
        Client UpdateClient(DatabaseContext _db, Client client);
    }
}
