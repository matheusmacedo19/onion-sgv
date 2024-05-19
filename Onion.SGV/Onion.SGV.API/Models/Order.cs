using System.ComponentModel.DataAnnotations;

namespace Onion.SGV.API.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(14)]
        public string ClientId { get; set; }
        public int ProductId {  get; set; }
        public virtual Client Client { get; set; }
        public virtual Product Product { get; set; }
        public DateTime OrderDate { get; set; }

    }
}
