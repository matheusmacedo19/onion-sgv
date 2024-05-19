using Onion.SGV.API.Data;
using Onion.SGV.API.Models;
using Onion.SGV.API.Services.Interfaces;

namespace Onion.SGV.API.Services
{
    public class ProductService : IProductService
    {
        private readonly MyDbContext _dbcontext;
        public ProductService(MyDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }
        public Product Get(int id)
        {
            throw new NotImplementedException();
        }

        public List<Product> GetAll()
        {
            throw new NotImplementedException();
        }

    }
}
