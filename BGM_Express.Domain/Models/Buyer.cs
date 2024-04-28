using System.ComponentModel.DataAnnotations;

namespace BGM_Express.Domain.Models
{
    public class Buyer
    {
        [Key]
        public int BuyerId { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string CompanyName { get; set; }

        //Navigation properties
        public ICollection<Order> Orders { get; set; }
    }
}
