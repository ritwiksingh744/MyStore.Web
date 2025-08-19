namespace MyStore.Models.DTOs
{
    public class ItemAddRequestModel
    {
        public Guid CategoryId { get; set; }
        public string ItemName { get; set; }
        public double Price { get; set; }
    }
}
