using System.ComponentModel.DataAnnotations;

namespace Onion.SGV.API.Models
{
    public class Client
    {
        [Key]
        [MaxLength(14)]
        public string Document { get; set; }
        public string SocialName { get; set; }
        [MaxLength(8)]
        public string Cep { get; set; }

        //Associações
        public ICollection<Order> Orders { get;} = new List<Order>();
    }
}
