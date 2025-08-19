using System.ComponentModel.DataAnnotations;

namespace MyStore.Data.Entity
{
    public class BaseEntity
    {
        [Key]
        public Guid Id { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}

