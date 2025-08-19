namespace MyStore.Models.DTOs
{
    public class CategoryDTO
    {
        public Guid Id { get; set; }
        public string CategoryName { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}
