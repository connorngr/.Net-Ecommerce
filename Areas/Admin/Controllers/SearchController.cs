using Innerglow_App.Areas.Identity.Data;
using Innerglow_App.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Innerglow_App.Areas.Admin.Controllers
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
            if (string.IsNullOrEmpty(keyword))
            {
                return PartialView("ListProductsSearchPartial", _context.Products.AsNoTracking().Include(x => x.Category).OrderByDescending(x => x.Id).ToList());
            }
            products = _context.Products.AsNoTracking().Include(x => x.Category).Where(x => x.ProductName.Contains(keyword)).OrderByDescending(x => x.Id).ToList();
            return PartialView("ListProductsSearchPartial", products);
        }
    }
}
