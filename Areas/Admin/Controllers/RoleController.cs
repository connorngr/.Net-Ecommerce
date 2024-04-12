using Microsoft.AspNetCore.Mvc;


namespace WebApp.Areas.Admin.Controllers
{
    public class RoleController : Controller
    {
        public async Task<IActionResult> Index()
        {
            return View();
        }
    }
}
