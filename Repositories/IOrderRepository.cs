using Innerglow_App.Models;

namespace Innerglow_App.Repositories
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetAllAsync();
        Task<Order> GetByIdAsync(int id);
        Task UpdateAsync(Order product);
    }
}
