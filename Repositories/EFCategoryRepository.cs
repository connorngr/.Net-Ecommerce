using Microsoft.EntityFrameworkCore;
using Innerglow_App.Areas.Identity.Data;
using Innerglow_App.Models;

namespace Innerglow_App.Repositories
{
    public class EFCategoryRepository : ICategoryRepository
    {
        private readonly UserContext _context;
        public EFCategoryRepository(UserContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            // return await _context.Categories.ToListAsync();
            return await _context.Categories.Where(x => x.isDeleted == true).ToListAsync();
        }
        public async Task<Category> GetByIdAsync(int id)
        {
            return await _context.Categories.FindAsync(id);
        }
        public async Task AddAsync(Category Category)
        {
            _context.Categories.Add(Category);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(Category Category)
        {
            _context.Categories.Update(Category);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var Category = await _context.Categories.FindAsync(id);
            Category.isDeleted = false;
            await _context.SaveChangesAsync();
        }
    }
}