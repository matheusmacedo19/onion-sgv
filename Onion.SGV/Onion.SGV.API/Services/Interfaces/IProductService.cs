using Onion.SGV.API.Models;

namespace Onion.SGV.API.Services.Interfaces
{
    public interface IProductService
    {
        Product Get(int id);
        List<Product> GetAll();
    }
}
