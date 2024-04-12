using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Areas.Identity.Data;
using WebApp.Data;
using WebApp.Repositories;


namespace WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class RoleAdminController : Controller
    {
        /*private readonly IRoleRepository _roleRepository;*/
        /*public RoleAdminController(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }*/
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<User> _userManager;
        public RoleAdminController(RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
        {
            this.roleManager = roleManager;
            _userManager = userManager;
        }

        /*public async Task<IActionResult> Index()
        {
            var roles = roleManager.Roles.ToList();
            return View(roles);
        }
        [HttpPost]*/
        public async Task<IActionResult> Index(string? email, string? RoleName)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    ModelState.AddModelError("", "Email not found!");
                }
                else
                {
                    ModelState.AddModelError("", "Add successfully");
                    await _userManager.AddToRoleAsync(user, RoleName);
                }
            }
            catch (Exception ex)
            {

            }
            var roles = roleManager.Roles.ToList();
            return View(roles);
        }
    }
}
