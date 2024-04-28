using System.Xml.Serialization;

namespace BGM_Express.Core.Dto
{
    [XmlRoot("Buyer")]
    public class BuyerDto
    {
        public int BuyerId { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string CompanyName { get; set; }
    }
}
