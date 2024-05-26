using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Numerics;
using System.Diagnostics.CodeAnalysis;

namespace Innerglow_App.Models
{
    [Table("Product")]
    public class Product
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(255)]
        public string ProductName { get; set; }
        [Required]
        public int Quantity { get; set; }
        public DateTime Time { get; set; } = DateTime.UtcNow;
        [Required]
        public int Price { get; set; }
        [MaybeNull]
        public int Discount { get; set; }
        [Required]
        public string Description { get; set; }
        [MaybeNull]
        public string Img_Url { get; set; }
        [MaybeNull]
        public string DetailProduct { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        [NotMapped]
        public string CategoryName { get; set; }
        public bool isDeleted { get; set; } = true;
    }
}
