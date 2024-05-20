using Microsoft.EntityFrameworkCore;
using Onion.SGV.API.Data;
using Onion.SGV.API.Models;
using Onion.SGV.API.Services.Interfaces;
using System.Reflection.Metadata;

namespace Onion.SGV.API.Services
{
    public class ClientService : IClientService
    {
        private readonly MyDbContext _dbContext;
        public ClientService(MyDbContext dbContext) 
        {
            _dbContext = dbContext;
        }
        public async void Add(Client client)
        {
            try
            {
                var clientTest = _dbContext.Clients.Where(x=>x.Document.Equals(client.Document)).FirstOrDefault();
                if (clientTest == null)
                {
                    await _dbContext.Clients.AddAsync(client);
                    await _dbContext.SaveChangesAsync();
                }

            }catch(Exception ex)
            {
                throw ex;
            }
        }
        public async Task<Client> Get(int document)
        {
            
            Client? client = await _dbContext.Clients.Include(x=> x.Orders).Where(x=>x.Document.Equals(document)).FirstOrDefaultAsync();
            
            if(client != null)
            {
                return client;
            }
            else
            {
                throw new Exception("Cliente não encontrado!");
            }

        }

        public async Task<IEnumerable<Client>> GetAll()
        {
            IEnumerable<Client> clients = await _dbContext.Clients.Include(s=>s.Orders).ThenInclude(x=>x.Product).ToListAsync();
            return clients;
        }
    }
}
