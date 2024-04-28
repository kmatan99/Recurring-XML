using System.Xml.Serialization;

namespace BGM_Express.Core.Dto
{
    [XmlRoot("Car")]
    public class CarDto
    {
        public int CarId { get; set; }
        public required string Make { get; set; }
        public required string Model { get; set; }
        public int Year { get; set; }
        public string? Color { get; set; }
    }
}
