using Onion.SGV.API.Models;

namespace Onion.SGV.API.Services.Interfaces
{
    public interface IProductService
    {
        Task<Product> Get(string id);
    }
}
