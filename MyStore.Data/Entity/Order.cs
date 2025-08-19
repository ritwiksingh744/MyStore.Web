namespace MyStore.Data.Entity
{
    public class Order : BaseEntity
    {
        public string OrderNumber { get; set; }
        public double Total { get; set; }
        public string CustomerName { get; set; }
        public DateTime OrderDate { get; set; }

    }
}
