using Onion.SGV.API.Data;
using Onion.SGV.API.Models;
using Onion.SGV.API.Services.Interfaces;

namespace Onion.SGV.API.Services
{
    public class ProductService : IProductService
    {
        private MyDbContext _dbcontext;
        public ProductService(MyDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }
        public Product Get(string nome)
        {
            Product? productResult = _dbcontext.Products.Where(p => p.Name.Equals(nome)).FirstOrDefault();

            if (productResult != null)
            {
                return productResult;
            }
            else
            {
                return productResult = new Product();
            }
        }

        public List<Product> GetAll()
        {
            throw new NotImplementedException();
        }

    }
}
