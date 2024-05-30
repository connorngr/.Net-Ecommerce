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

        [HttpPost]
        public IActionResult FindOrder(string keyword)
        {
            List<Order> products = new List<Order>();
            if (string.IsNullOrEmpty(keyword) || keyword.Length < 1)
            {
                return PartialView("ListOrdersSearchPartial", null);
            }
            products = _context.Orders.AsNoTracking().Include(x => x.OrderStatus).Where(x => x.OrderStatus.StatusName.Contains(keyword)).OrderByDescending(x => x.Id).ToList();
            if (products == null)
            {
                return PartialView("ListOrdersSearchPartial", null);
            }
            else
            {
                return PartialView("ListOrdersSearchPartial", products);
            }
        }
    }
}
