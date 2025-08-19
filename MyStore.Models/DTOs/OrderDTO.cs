namespace MyStore.Models.DTOs
{
    public class OrderDTO
    {
        public Guid Id { get; set; }
        public string OrderNumber { get; set; }
        public double Total { get; set; }
        public string CustomerName { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
