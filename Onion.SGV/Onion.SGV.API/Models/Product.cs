using System.ComponentModel.DataAnnotations;

namespace Onion.SGV.API.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public virtual List<Order> Orders { get; set; }

        public Product()
        {
            
        }
        public Product(string name, double price)
        {
            Name = name;
            Price = price;
        }
    }
}
