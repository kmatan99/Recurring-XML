using BGM_Express.Core.Dto;
using System.Xml.Serialization;

namespace BGM_Express.Core.Command
{
    [XmlRoot("Order")]
    public class UpsertOrderCommand
    {
        public int OrderId { get; set; } = 0;
        public DateTime OrderDate { get; set; }
        public float Price { get; set; }
        public CarDto? Car { get; set; }
        public BuyerDto? Buyer { get; set; }
        public bool IsProcessed { get; set; }
    }
}
