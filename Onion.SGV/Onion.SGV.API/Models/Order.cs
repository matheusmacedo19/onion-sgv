using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Onion.SGV.API.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(14)]
        public string? ClientId { get; set; }
        public int? ProductId {  get; set; }
        [ForeignKey("ClientId")]
        public virtual Client? Client { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product? Product { get; set; }
        public DateTime OrderDate { get; set; }

    }
}
