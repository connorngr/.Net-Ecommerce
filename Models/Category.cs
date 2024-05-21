using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Innerglow_App.Models
{
    [Table("Category")]
    public class Category
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(255)]
        public string CategoryName { get; set; }
        public List<Product> Products { get; set; }
    }
}
