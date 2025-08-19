namespace MyStore.Models.DTOs
{
    public class OrderRequestModel
    {
        public DateTime OrderDate { get; set; }
        public string CustomerName { get; set; }   
        public double ItemTotal { get; set; }
        public List<ItemRequestModel> Items { get; set; }
    }
}
