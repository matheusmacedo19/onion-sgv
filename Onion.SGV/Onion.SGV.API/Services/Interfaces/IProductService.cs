using Onion.SGV.API.Models;

namespace Onion.SGV.API.Services.Interfaces
{
    public interface IProductService
    {
        Product Get(string id);
        List<Product> GetAll();
    }
}
