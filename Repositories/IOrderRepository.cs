using Innerglow_App.Models;

namespace Innerglow_App.Repositories
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetAllAsync();
    }
}
