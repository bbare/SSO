using DataAccessLayer.Database;
using DataAccessLayer.Models;
using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class ClientRepository
    {
        public Client CreateClient(DatabaseContext _db, Client client)
        {
            _db.Entry(client).State = EntityState.Added;
            return client;
        }

        public Client DeleteClient(DatabaseContext _db, Guid Id)
        {
            var client= _db.Clients
                .Where(c => c.Id == Id)
                .FirstOrDefault<Client>();
            if (client == null)
                return null;
            _db.Entry(client).State = EntityState.Deleted;
            return client;

        }

        public Client GetClient(DatabaseContext _db, Guid Id)
        {
            var client = _db.Clients
                .Where(c => c.Id == Id)
                .FirstOrDefault<Client>();
            return client;
        }

        public Client UpdateClient(DatabaseContext _db, Client client)
        {
            client.UpdatedAt = DateTime.UtcNow;
            _db.Entry(client).State = EntityState.Modified;
            return client;

        }
        
        public bool IsClientDisabled(DatabaseContext _db, Client client)
        {
            var result = GetClient(_db, client.Id);
            if (result.Disabled) {
                return true;
            }
            return false;

        }

        public bool IsExistingClient(DatabaseContext _db, Client client)
        {
            var result = GetClient(_db, client.Id);
            if (result != null) {
                return true;
            }
            return false;
        }



    }
}
