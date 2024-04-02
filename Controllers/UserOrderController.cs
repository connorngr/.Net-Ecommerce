using Microsoft.AspNetCore.Mvc;
using WebApp.Repositories;

namespace WebApp.Controllers
{
    public class UserOrderController : Controller
    {
        private readonly IUserOrderRepository _repository;
        public UserOrderController(IUserOrderRepository repository)
        {
            _repository = repository;
        }

        public async Task<IActionResult> UserOrders()
        {
            var UserOrders = await _repository.UserOrders();
            return View(UserOrders);
        }
    }
}
