using Microsoft.EntityFrameworkCore;
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
        public async Task<Product> Get(string nome)
        {
            try
            {
                Product? productResult = await _dbcontext.Products.Where(p => p.Name.Equals(nome)).FirstOrDefaultAsync();

                if (productResult != null)
                {
                    return productResult;
                }
                else
                {
                    return productResult = new Product();
                }
            }catch (Exception ex)
            {
                throw;
            }
        }
    }
}
