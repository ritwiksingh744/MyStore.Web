
namespace MyStore.Models.DTOs
{
    public class OrderResponseDTO
    {
        public Guid OrderId { get; set; }
        public string OrderNumber { get; set; }
        public double Total { get; set; }
        public string CustomerName { get; set; }
        public DateTime OrderDate { get; set; }
        public IEnumerable<OrderedItemDTO> Items { get; set; }
        public IEnumerable<CategoryDTO> Categories { get; set; }
        public IEnumerable<ItemDTO> CategoryItems { get; set; }

    }
}
