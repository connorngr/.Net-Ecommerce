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
        [DisplayName("Tên sản phẩm")]
        public string ProductName { get; set; }
        [Required]
        [DisplayName("Số lượng")]
        [Range(0, 1000, ErrorMessage = "{0} phải lớn hơn {1} và nhỏ hơn {2}")]
        public int Quantity { get; set; }
        public DateTime Time { get; set; } = DateTime.UtcNow;
        [Required]
        [DisplayName("Giá")]
        [Range(1000,100000000, ErrorMessage = "{0} phải lớn hơn {1} và nhỏ hơn {2}")]
        public int Price { get; set; }
        [MaybeNull]
        [DisplayName("Sau khi giảm giá")]
        [Range(1000, 100000000, ErrorMessage = "{0} phải lớn hơn {1} và nhỏ hơn {2}")]
        public int Discount { get; set; }
        
        [Required]
        [DisplayName("Mô tả sản phẩm")]
        public string Description { get; set; }
        [MaybeNull]
        public string Img_Url { get; set; }

        [MaybeNull]
        public List<string> Img_Urls { get; set; }
        [MaybeNull]
        [DisplayName("Chi tiết sản phẩm")]
        public string DetailProduct { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        [NotMapped]
        public string CategoryName { get; set; }
        public bool isDeleted { get; set; } = true;
    }
}
