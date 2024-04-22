using Humanizer.Localisation;

namespace Sang3_Nhom2_WebBanThucPhamChucNang.Models
{
    public class ProductDisplayModel
    {
        public IEnumerable<Product> Products { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public string Search { get; set; } = "";
        public int CategoryID { get; set; } = 0;
    }
}
