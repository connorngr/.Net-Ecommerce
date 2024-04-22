using Sang3_Nhom2_WebBanThucPhamChucNang.Models;

namespace Sang3_Nhom2_WebBanThucPhamChucNang.Repositories
{
    public interface IUserOrderRepository
    {
        Task<IEnumerable<Order>> UserOrders();
    }
}