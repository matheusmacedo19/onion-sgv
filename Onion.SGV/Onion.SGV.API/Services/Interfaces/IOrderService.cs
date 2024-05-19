using Onion.SGV.API.Models;

namespace Onion.SGV.API.Services.Interfaces
{
    public interface IOrderService
    {
        void OpenTransaction();
        void CommitTransaction();
        void RollTransaction();
        void Add(Order order);
        Order Get(int id);
        List<Order> GetAll();
    }
}
