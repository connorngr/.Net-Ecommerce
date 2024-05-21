using Innerglow_App.Models;

namespace Innerglow_App.Repositories
{
    public interface IUserOrderRepository
    {
        Task<IEnumerable<Order>> UserOrders();
    }
}