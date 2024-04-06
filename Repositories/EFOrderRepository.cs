using WebApp.Models;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;

namespace WebApp.Repositories
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
            return await _context.Orders
            .Include(p => p.OrderDetail) // Include thông tin về category
            .ToListAsync();
        }
    }
}
