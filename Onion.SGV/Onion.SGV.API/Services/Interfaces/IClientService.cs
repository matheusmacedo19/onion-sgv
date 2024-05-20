using Onion.SGV.API.Models;

namespace Onion.SGV.API.Services.Interfaces
{
    public interface IClientService
    {
        void Add(Client client);
        Task<Client> Get(int document);
        Task<IEnumerable<Client>> GetAll();
    }
}
