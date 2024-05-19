using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sang3_Nhom2_WebBanThucPhamChucNang.Data;
using Sang3_Nhom2_WebBanThucPhamChucNang.Models;

namespace Sang3_Nhom2_WebBanThucPhamChucNang.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SearchController : Controller
    {
        private readonly UserContext _context;
        public SearchController(UserContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult FindProduct(string keyword)
        {
            List<Product> products = new List<Product>();
            if (string.IsNullOrEmpty(keyword) || keyword.Length < 1)
            {
                return PartialView("ListProductsSearchPartial", null);
            }
            products = _context.Products.AsNoTracking().Include(x => x.Category).Where(x => x.ProductName.Contains(keyword)).OrderByDescending(x => x.Id).ToList();
            if (products == null)
            {
                return PartialView("ListProductsSearchPartial", null);
            }
            else
            {
                return PartialView("ListProductsSearchPartial", products);
            }
        }
    }
}
