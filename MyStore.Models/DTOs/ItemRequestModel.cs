namespace MyStore.Models.DTOs
{
    public class ItemRequestModel
    {
        public Guid ItemId { get; set; }
        public Guid CategoryId { get; set; }
        public int Quantity { get; set; }
        public double Rate { get; set; }

    }
}
