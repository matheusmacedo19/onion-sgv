using Onion.SGV.API.Models;

namespace Onion.SGV.API.Services.Interfaces
{
    public interface IClientService
    {
        void OpenTransaction();
        void CommitTransaction();
        void RollTransaction();
        void Add(Client client);
        Client Get(int document);
        List<Client> GetAll();
    }
}
