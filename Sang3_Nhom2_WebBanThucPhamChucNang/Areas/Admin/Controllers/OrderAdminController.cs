using Sang3_Nhom2_WebBanThucPhamChucNang.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Sang3_Nhom2_WebBanThucPhamChucNang.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin, Employee")]
    public class OrderAdminController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        public OrderAdminController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }
        public async Task<IActionResult> Index()
        {
            var orders = await _orderRepository.GetAllAsync();
            return View(orders);
        }
    }
}
