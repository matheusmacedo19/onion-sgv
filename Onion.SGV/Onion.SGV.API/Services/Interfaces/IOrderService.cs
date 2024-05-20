using Onion.SGV.API.Models;

namespace Onion.SGV.API.Services.Interfaces
{
    public interface IOrderService
    {
        void Add(Order order);
        Task<Order> Get(int id);
        Task<IEnumerable<Order>> GetAll();
    }
}
