namespace MyStore.Data.Entity
{
    public class OrderedItem : BaseEntity
    {
        public Guid OrderId { get; set; }
        public Guid ItemId { get; set; }
        public Guid CategoryId { get; set; }
        public int Quantity { get; set; }
        public double Rate { get; set; }
    }
}
