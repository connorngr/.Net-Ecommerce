using Microsoft.EntityFrameworkCore;
using Innerglow_App.Areas.Identity.Data;
using Innerglow_App.Models;

namespace Innerglow_App.Repositories
{

    public class EFOrderRepository : IOrderRepository
    {
        public readonly UserContext _context;

        public EFOrderRepository(UserContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            IEnumerable<Order> orders = await _context.Orders
                            .Include(x => x.OrderStatus)
                            .Include(x => x.OrderDetail)
                            .ThenInclude(x => x.Product)
            .ThenInclude(x => x.Category)
                            .ToListAsync();
            return orders;
        }

        public async Task<Order> GetByIdAsync(int id)
        {
            // return await _context.Products.FindAsync(id);
            // lấy thông tin kèm theo category
            return await _context.Orders.Include(p => p.OrderStatus).FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task UpdateAsync(Order product)
        {
            _context.Orders.Update(product);
            await _context.SaveChangesAsync();
        }
    }
}
