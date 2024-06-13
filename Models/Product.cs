using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Numerics;
using System.Diagnostics.CodeAnalysis;
using System.ComponentModel;

namespace Innerglow_App.Models
{
    [Table("Product")]
    public class Product
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100,MinimumLength=3)]
        [DisplayName("Name product")]
        public string ProductName { get; set; }
        [Required]
        [DisplayName("Quantity")]
        [Range(0, 1000, ErrorMessage = "{0} must be greater than {1} and less than {2}")]
        public int Quantity { get; set; }
        public DateTime Time { get; set; } = DateTime.UtcNow;
        [Required]
        [DisplayName("Giá")]
        [Range(1000,100000000, ErrorMessage = "{0} must be greater than {1} and less than {2}")]
        public int Price { get; set; }
        [MaybeNull]
        [DisplayName("Sau khi giảm giá")]
        [Range(1000, 100000000, ErrorMessage = "{0} must be greater than {1} and less than {2}")]
        public int Discount { get; set; }
        
        [Required]
        [DisplayName("Description product")]
        public string Description { get; set; }
        [MaybeNull]
        public string Img_Url { get; set; }

        [MaybeNull]
        public List<string> Img_Urls { get; set; }
        [MaybeNull]
        [DisplayName("Details product")]
        public string DetailProduct { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        [NotMapped]
        public string CategoryName { get; set; }
        public bool isDeleted { get; set; } = true;
    }
}
