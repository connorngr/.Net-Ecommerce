using Sang3_Nhom2_WebBanThucPhamChucNang.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using System.Text.Encodings.Web;
using System.Text;
using static Sang3_Nhom2_WebBanThucPhamChucNang.Areas.Identity.Pages.Account.RegisterModel;
using System.Data;
using NuGet.Configuration;
using Sang3_Nhom2_WebBanThucPhamChucNang.Data;
using Sang3_Nhom2_WebBanThucPhamChucNang.Areas.Identity.Data;

namespace Sang3_Nhom2_WebBanThucPhamChucNang.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AccountAdminController : Controller
    {
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;
        private readonly IUserStore<User> _userStore;
        string[] args;


        public AccountAdminController(RoleManager<IdentityRole> roleManager, UserManager<User> userManager, IUserStore<User> userStore, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _userStore = userStore;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> Index()
        {
            var builder = WebApplication.CreateBuilder(args);
            var optionsBuilder = new DbContextOptionsBuilder<UserContext>();
            optionsBuilder.UseSqlServer(builder.Configuration.GetConnectionString("UserContextConnection"));
            var _context = new UserContext(optionsBuilder.Options);
            
            var users = _context.Users.ToList();
            var roles = _context.Roles.ToList();

            var userViewModels = new List<User>();
            foreach (var user in users)
            {
                var userViewModel = new User();
                userViewModel.Id = user.Id;
                userViewModel.FullName = user.FullName;
                userViewModel.PhoneNumber = user.PhoneNumber;

                userViewModel.Email = user.Email;

                var rolesForUser = _context.UserRoles.Where(x => x.UserId == user.Id).ToList();
                foreach (var role in rolesForUser)
                {
                    userViewModel.Role = roles.Where(x => x.Id == role.RoleId).FirstOrDefault().Name;
                }

                userViewModels.Add(userViewModel);
            }
            userViewModels = userViewModels.OrderByDescending(x => x.Id).ToList();
            return View(userViewModels);
        }

        public async Task<IActionResult> Add()
        {
            var builder = WebApplication.CreateBuilder(args);
            var optionsBuilder = new DbContextOptionsBuilder<UserContext>();
            optionsBuilder.UseSqlServer(builder.Configuration.GetConnectionString("UserContextConnection"));
            var _context = new UserContext(optionsBuilder.Options);
            ViewBag.Role = new SelectList(_context.Roles.ToList(), "Name", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(InputModel model)
        {
            var builder = WebApplication.CreateBuilder(args);
            var optionsBuilder = new DbContextOptionsBuilder<UserContext>();
            optionsBuilder.UseSqlServer(builder.Configuration.GetConnectionString("UserContextConnection"));
            var _context = new UserContext(optionsBuilder.Options);
            ViewBag.Role = new SelectList(_context.Roles.ToList(), "Name", "Name");
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    Email = model.Email,
                    FullName = model.FullName,
                    PhoneNumber = model.PhoneNumber
                };

                await _userStore.SetUserNameAsync(user, model.Email, CancellationToken.None);
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, model.Role);
                    return RedirectToAction("Index", "AccountAdmin");
                }
            }
            return View(model);
        }

        public async Task<IActionResult> Edit(string id)
        {
            var builder = WebApplication.CreateBuilder(args);
            var optionsBuilder = new DbContextOptionsBuilder<UserContext>();
            optionsBuilder.UseSqlServer(builder.Configuration.GetConnectionString("UserContextConnection"));
            var _context = new UserContext(optionsBuilder.Options);
            var userToUpdate = await _context.Users.FindAsync(id);
            if (userToUpdate == null)
            {
                return NotFound();
            }
            ViewBag.Role = new SelectList(_context.Roles.ToList(), "Name", "Name");
            return View(userToUpdate);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(string id, User model)
        {
            var builder = WebApplication.CreateBuilder(args);
            var optionsBuilder = new DbContextOptionsBuilder<UserContext>();
            optionsBuilder.UseSqlServer(builder.Configuration.GetConnectionString("UserContextConnection"));
            var _context = new UserContext(optionsBuilder.Options);
            ViewBag.Role = new SelectList(_context.Roles.ToList(), "Name", "Name");
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(id);
                user.Email = model.Email;
                user.FullName = model.FullName;
                user.PhoneNumber = model.PhoneNumber;
                await _userStore.SetUserNameAsync(user, model.Email, CancellationToken.None);
                var userRoles = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, userRoles);
                await _userManager.AddToRoleAsync(user, model.Role);
                await _userManager.UpdateAsync(user);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public async Task<IActionResult> Delete(string id)
        {
            var builder = WebApplication.CreateBuilder(args);
            var optionsBuilder = new DbContextOptionsBuilder<UserContext>();
            optionsBuilder.UseSqlServer(builder.Configuration.GetConnectionString("UserContextConnection"));
            var _context = new UserContext(optionsBuilder.Options);
            var userToUpdate = await _context.Users.FindAsync(id);
            if (userToUpdate == null)
            {
                return NotFound();
            }
            return View(userToUpdate);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            await _userManager.DeleteAsync(user);
            return RedirectToAction("Index");
        }
    }
}
