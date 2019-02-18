using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Repositories;
using DataAccessLayer.Models;
using DataAccessLayer.Database;

namespace ServiceLayer.Services
{
    public class ClientService : IClientService
    {
        private ClientRepository _clientRepo;

        public ClientService() {
            _clientRepo = new ClientRepository();
        }

        public Client CreateClient(DatabaseContext _db, Client client)
        {
            if (_clientRepo.IsExistingClient(_db, client))
            {
                Console.WriteLine("Client exists");
                return null;
            }
            return _clientRepo.CreateClient(_db, client);
        }

        public Client DeleteClient(DatabaseContext _db, Guid Id)
        {
            return _clientRepo.DeleteClient(_db, Id);
        }

        public Client GetClient(DatabaseContext _db, Guid Id)
        {
            return _clientRepo.GetClient(_db, Id);
        }

        public Client UpdateClient(DatabaseContext _db, Client client)
        {
            return _clientRepo.UpdateClient(_db, client);
        }


    }
}
