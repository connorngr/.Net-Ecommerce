using Innerglow_App.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Innerglow_App.Controllers
{
    public class UserOrderController : Controller
    {
        private readonly IUserOrderRepository _repository;
        public UserOrderController(IUserOrderRepository repository)
        {
            _repository = repository;
        }
        [Authorize]
        public async Task<IActionResult> UserOrders()
        {
            var UserOrders = await _repository.UserOrders();
            return View(UserOrders);
        }
    }
}
