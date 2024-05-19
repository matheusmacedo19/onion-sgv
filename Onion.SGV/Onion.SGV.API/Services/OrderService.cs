using Onion.SGV.API.Data;
using Onion.SGV.API.Models;
using Onion.SGV.API.Services.Interfaces;

namespace Onion.SGV.API.Services
{
    public class OrderService : IOrderService
    {
        private readonly MyDbContext _dbContext;
        public OrderService(MyDbContext dbContext) 
        {
            _dbContext = dbContext;
        }
        public void Add(Order order)
        {
            if(_dbContext.Orders.Find(order.Id) == null)
            {
                _dbContext.Orders.Add(order);
            }
            else
            {
                throw new Exception("Pedido de número: " + order.Id + " ja existe!");
            }
        }

        public Order Get(int id)
        {
            Order? order = _dbContext.Orders.Find(id);
            
            if(order != null)
            {
                return order;
            }
            else
            {
                 return new Order();
            }
        }

        public List<Order> GetAll()
        {
            List<Order> list = _dbContext.Orders.OrderByDescending(x => x.Id).ToList();
            return list;
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
