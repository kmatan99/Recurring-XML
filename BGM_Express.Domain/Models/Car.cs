using System.ComponentModel.DataAnnotations;

namespace BGM_Express.Domain.Models
{
    public class Car
    {
        [Key]
        public int CarId { get; set; }
        public required string Make { get; set; }
        public required string Model { get; set; }
        public int Year { get; set; }
        public string? Color { get; set; }

        //Navigation properties
        public Order Order { get; set; }
    }
}
