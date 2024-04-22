using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sang3_Nhom2_WebBanThucPhamChucNang.Models
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
