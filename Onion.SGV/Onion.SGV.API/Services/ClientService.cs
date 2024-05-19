using Onion.SGV.API.Data;
using Onion.SGV.API.Models;
using Onion.SGV.API.Services.Interfaces;

namespace Onion.SGV.API.Services
{
    public class ClientService : IClientService
    {
        private readonly MyDbContext _dbContext;
        public ClientService(MyDbContext dbContext) 
        {
            _dbContext = dbContext;
        }
        public void Add(Client client)
        {
            if(_dbContext.Clients.Find(client.Document) != null)
            {
                _dbContext.Clients.Add(client);
            }
            else
            {
                throw new Exception("Cliente com documento: "+client.Document+" ja existe!");
            }
        }
        public Client Get(int document)
        {
            return _dbContext.Clients.Where(x=> x.Document.Equals(document)).FirstOrDefault();
        }

        public List<Client> GetAll()
        {
            return _dbContext.Clients.ToList();
        }

        public void CommitTransaction()
        {
            _dbContext.Database.CommitTransaction();
        }


        public void OpenTransaction()
        {
            _dbContext.Database.BeginTransaction();
        }

        public void RollTransaction()
        {
            _dbContext.Database.RollbackTransaction();
        }
    }
}
