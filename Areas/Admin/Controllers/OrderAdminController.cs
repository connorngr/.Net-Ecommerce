using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Innerglow_App.Repositories;
using Innerglow_App.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Innerglow_App.Areas.Identity.Data;

namespace Innerglow_App.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin, Employee")]
    public class OrderAdminController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly UserContext _context;

        public OrderAdminController(IOrderRepository orderRepository, UserContext context)
        {
            _orderRepository = orderRepository;
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var orders = await _orderRepository.GetAllAsync();
            var categories = await _context.OrderStatus.ToListAsync();
            ViewBag.OrderStatusId = new SelectList(categories, "Id", "StatusName");
            return View(orders);
        }

        public IActionResult Filtter(int CategoryID = 0)
        {
            var url = $"/Admin/OrderAdmin?OrderStatusID={CategoryID}";
            if (CategoryID == 0)
            {
                url = $"/Admin/OrderAdmin";
            }
            return Json(new { status = "success", redirectUrl = url });
        }

        public async Task<IActionResult> Update(int id)
        {
            var product = await _orderRepository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            var categories = await _context.OrderStatus.ToListAsync();
            ViewBag.OrderStatusId = new SelectList(categories, "Id", "StatusName");
            return View(product);
        }
        // Xử lý cập nhật sản phẩm
        [HttpPost]
        public async Task<IActionResult> Update(int id, Order product, IFormFile Img_Url)
        {
            /*ModelState.Remove("ImageUrl");*/ // Loại bỏ xác thực ModelState cho ImageUrl

            if (id != product.Id)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                var existingProduct = await _orderRepository.GetByIdAsync(id); // Giả định có phương thức GetByIdAsync

                existingProduct.OrderDate = product.OrderDate;
                existingProduct.ShippingAddress = product.ShippingAddress;
                existingProduct.PhoneNumber = product.PhoneNumber;
                existingProduct.Notes = product.Notes;
                existingProduct.OrderStatusId = product.OrderStatusId;
                await _orderRepository.UpdateAsync(existingProduct);
                return RedirectToAction(nameof(Index));
            }
            var categories = await _context.OrderStatus.ToListAsync();
            ViewBag.OrderStatusId = new SelectList(categories, "Id", "StatusName");
            return View(product);
        }
    }
}
