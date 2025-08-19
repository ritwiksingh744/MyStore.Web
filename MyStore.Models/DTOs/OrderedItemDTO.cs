namespace MyStore.Models.DTOs
{
    public class OrderedItemDTO
    {
        public Guid Id { get; set; }
        public Guid ItemId { get; set; }
        public Guid CategoryId { get; set; }
        public int Quantity { get; set; }
        public double Rate { get; set; }
    }
}
