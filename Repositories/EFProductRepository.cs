using Microsoft.EntityFrameworkCore;
using Innerglow_App.Areas.Identity.Data;
using Innerglow_App.Models;

namespace Innerglow_App.Repositories
{
    public class EFProductRepository : IProductRepository
    {
        public readonly UserContext _context;
        public EFProductRepository(UserContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            // return await _context.Products.ToListAsync();
            return await _context.Products
            .Include(p => p.Category).Where(x => x.isDeleted == true) // Include thông tin về category
            .ToListAsync();
        }
        public async Task<Product> GetByIdAsync(int id)
        {
            // return await _context.Products.FindAsync(id);
            // lấy thông tin kèm theo category
            return await _context.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == id);
        }
        public async Task AddAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            product.isDeleted = false;
            await _context.SaveChangesAsync();
        }
    }
}
