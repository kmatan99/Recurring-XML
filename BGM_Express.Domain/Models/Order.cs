using System.ComponentModel.DataAnnotations;

namespace BGM_Express.Domain.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public float Price { get; set; }
        public bool IsProcessed { get; set; }

        // Navigation properties
        public int CarId { get; set; }
        public Car Car { get; set; }
        public int BuyerId { get; set; }
        public Buyer Buyer { get; set; }
    }
}
