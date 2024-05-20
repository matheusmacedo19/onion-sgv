using System.ComponentModel.DataAnnotations;

namespace Onion.SGV.API.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public ICollection<Order> Orders { get;} = new List<Order>();
    }
}
