namespace BGM_Express.Core.Dto
{
    public class OrderDto
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public float Price { get; set; }
        public bool IsProcessed { get; set; }
    }
}
