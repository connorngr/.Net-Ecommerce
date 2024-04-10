using WebApp.Models;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.Areas.Identity.Data;

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
            IEnumerable<Order> orders = await _context.Orders
                            .Include(x => x.OrderStatus)
                            .Include(x => x.OrderDetail)
                            .ThenInclude(x => x.Product)
            .ThenInclude(x => x.Category)
                            .ToListAsync();
            return orders;
        }
    }
}
