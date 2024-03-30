using Microsoft.EntityFrameworkCore;
using System.IO;
using WebApp.Data;
using WebApp.Models;
using static System.Reflection.Metadata.BlobBuilder;

namespace WebApp.Repositories
{
    public class HomeRepository : IHomeRepository
    {
        private readonly UserContext _db;

        public HomeRepository(UserContext db)
        {
            _db = db;
        }
        public async Task<IEnumerable<Category>> Categories()
        {
            return await _db.Categories.ToListAsync();
        }
        public async Task<IEnumerable<Product>> GetProducts(string Search = "", int categoryID = 0)
        {
            IEnumerable<Product> products = await (from product in _db.Products
                            join category in _db.Categories
                            on product.CategoryId equals category.Id
            where string.IsNullOrWhiteSpace(Search) || (product != null && product.ProductName.ToLower().Contains(Search))
            select new Product
            {
                                Id = product.Id,
                                ProductName = product.ProductName,
                                Img_Url = product.Img_Url,
                                Description = product.Description,
                                Price = product.Price,
                                CategoryName = category.CategoryName,
                                CategoryId = product.CategoryId
                            }).ToListAsync();
            if (categoryID > 0)
            {
                
                products = products.Where(a => a.CategoryId == categoryID).ToList();
            }
            return products;
        }
    }
}
