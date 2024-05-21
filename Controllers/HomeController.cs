using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using Innerglow_App.Services;
using Innerglow_App.Repositories;
using Innerglow_App.Models;

namespace Innerglow_App.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHomeRepository _homeRepository;
        private readonly IProductRepository _productRepository;
        public HomeController(ILogger<HomeController> logger, IHomeRepository homeRepository,
            IProductRepository productRepository)
        {
            _homeRepository = homeRepository;
            _logger = logger;
            _productRepository = productRepository;

        }

        public async Task<IActionResult> Index(string Search = "", int CategoryID = 0)
        {
            IEnumerable<Product> products = await _homeRepository.GetProducts(Search, CategoryID);
            IEnumerable<Category> categories = await _homeRepository.Categories();
            ProductDisplayModel productDisplayModel = new ProductDisplayModel
            {
                Products = products,
                Categories = categories,
                Search = Search,
                CategoryID = CategoryID
            };
            return View(productDisplayModel);
        }

        public async Task<IActionResult> Display(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        /* public IActionResult Privacy()
         {
             return View();
         }

         [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
         public IActionResult Error()
         {
             return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
         }*/
    }
}
