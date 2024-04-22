using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Numerics;
using System.Diagnostics.CodeAnalysis;

namespace Sang3_Nhom2_WebBanThucPhamChucNang.Models
{
    [Table("Product")]
    public class Product
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(255)]
        public string ProductName { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public int Price { get; set; }
        [MaybeNull]
        public string Img_Url { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        [NotMapped]
        public string CategoryName { get; set; }
    }
}
