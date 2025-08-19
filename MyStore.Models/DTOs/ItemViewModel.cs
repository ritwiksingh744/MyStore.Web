namespace MyStore.Models.DTOs
{
    public class ItemViewModel
    {
        public Guid Id { get; set; }
        public string ItemName { get; set; }
        public double Price { get; set; }
        public Guid CategoryId { get; set; }
        public DateTime CreatedOn { get; set; }
        public CategoryDTO Category { get; set; }
    }
}
