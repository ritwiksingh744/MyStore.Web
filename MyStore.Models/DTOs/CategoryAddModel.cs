using System.ComponentModel.DataAnnotations;

namespace MyStore.Models.DTOs
{
    public class CategoryAddModel
    {
        [Required(ErrorMessage ="Category name is required.")]
        public string CategoryName { get; set; }
    }
}
