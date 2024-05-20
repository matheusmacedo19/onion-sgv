using Microsoft.EntityFrameworkCore;
using NuGet.Versioning;
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
            try
            {
                var orderExists = _dbContext.Orders.Any(x => x.Id == order.Id);
                if (!orderExists)
                {
                    _dbContext.Orders.Add(order);
                    _dbContext.SaveChanges();
                }
                else
                {
                    throw new InvalidOperationException("Pedido de número: " + order.Id + " já existe!");
                }
                
            }catch(Exception ex) 
            {
                throw;
            }
        }

        public async Task<Order> Get(int id)
        {
            try
            {
                Order? order = await _dbContext.Orders.Include(x => x.Product).Include(x=> x.Client).Where( x => x.Id == id).FirstOrDefaultAsync();
            
                if(order != null)
                {
                    return order;
                }
                else
                {
                     return new Order();
                }

            }catch(Exception ex)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Order>> GetAll()
        {
            try
            {
                var list = await _dbContext.Orders.Include(s=>s.Client).Include(s=>s.Product).ToListAsync();
                return list;

            }catch(Exception ex)
            {
                throw;
            }
        }
    }
}
