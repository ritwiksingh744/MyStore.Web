using System.ComponentModel.DataAnnotations.Schema;

namespace MyStore.Data.Entity
{
    public class Items : BaseEntity
    {
        public string ItemName { get; set; }
        public double Price { get; set; }
        public Guid CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }
    }
}
