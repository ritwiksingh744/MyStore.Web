namespace MyStore.Models.DTOs
{
    public class ItemDTO
    {
        public Guid Id { get; set; }
        public string ItemName { get; set; }
        public double Price { get; set; }
        public Guid CategoryId { get; set; }
    }
}
