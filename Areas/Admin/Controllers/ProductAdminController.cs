using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PagedList.Core;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System;
using Innerglow_App.Areas.Identity.Data;
using Innerglow_App.Repositories;
using Innerglow_App.Models;

namespace Innerglow_App.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = "Admin, Employee")]
    public class ProductAdminController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly UserContext _context;
        private readonly ILogger<Product> _logger;
        public ProductAdminController(IProductRepository productRepository,

        ICategoryRepository categoryRepository, UserContext context)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _context = context;
        }
        // Hiển thị danh sách sản phẩm
        public IActionResult Index(int page = 1, int CategoryID = 0)
        {
            var pageNumber = page;
            var pageSize = 15;
            List<Product> products = new List<Product>();
            if (CategoryID != 0)
            {
                products = _context.Products.AsNoTracking().Where(x => x.CategoryId == CategoryID).Include(x => x.Category).OrderByDescending(x => x.Id).ToList();
            }
            else
            {
                products = _context.Products.AsNoTracking().Include(x => x.Category).OrderByDescending(x => x.Id).ToList();
            }
            ViewBag.CurrentCate = CategoryID;
            ViewBag.Categories = new SelectList(_context.Categories, "Id", "CategoryName", CategoryID);

            PagedList<Product> product = new PagedList<Product>(products.AsQueryable(), pageNumber, pageSize);

            return View(product);
        }

        public IActionResult Filtter(int CategoryID = 0)
        {
            var url = $"/Admin/ProductAdmin?CategoryID={CategoryID}";
            if (CategoryID == 0)
            {
                url = $"/Admin/ProductAdmin";
            }
            return Json(new { status = "success", redirectUrl = url });
        }

        // Hiển thị form thêm sản phẩm mới
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> Add()
        {
            var categories = await _categoryRepository.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "CategoryName");
            return View();
        }
        // Xử lý thêm sản phẩm mới
        [HttpPost]
        public async Task<IActionResult> Add(Product product, IFormFile imageUrl)
        {
            if (!ModelState.IsValid)
            {
                if (imageUrl != null)
                {
                    // Lưu hình ảnh đại diện tham khảo bài 02 hàm SaveImage
                    product.Img_Url = await SaveImage(imageUrl);
                }
                await _productRepository.AddAsync(product);
                return RedirectToAction(nameof(Index));
            }
            // Nếu ModelState không hợp lệ, hiển thị form với dữ liệu đã nhập
            var categories = await _categoryRepository.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "CategoryName");
            return View(product);
        }
        // Viết thêm hàm SaveImage (tham khảo bài 02)
        private async Task<string> SaveImage(IFormFile image)
        {
            var savePath = Path.Combine("wwwroot/images/", image.FileName); //Thay đổi đường dẫn theo cấu hình của bạn

            _logger.LogInformation("wwwroot/images/" + image.FileName);
            using (var fileStream = new FileStream(savePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
                _logger.LogInformation("Save finish.");
            }
            return image.FileName; // Trả về đường dẫn tương đối
        }
        public ActionResult UploadImage(IFormFile upload)
        {
            if (upload != null && upload.Length > 0)
            {
                var filename = DateTime.Now.ToString("yyyyMMddHHmmss") + upload.FileName;
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/"
                    , filename);
                var stream = new FileStream(path, FileMode.Create);
                upload.CopyToAsync(stream);

                return new JsonResult(new { path = "/uploads/" + filename });
            }
            return RedirectToAction("Add");

        }
        public async Task<IActionResult> Display(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }
        // Hiển thị form cập nhật sản phẩm
        public async Task<IActionResult> Update(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            var categories = await _categoryRepository.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "CategoryName");
            return View(product);
        }
        // Xử lý cập nhật sản phẩm
        [HttpPost]
        public async Task<IActionResult> Update(int id, Product product, IFormFile Img_Url)
        {
            /*ModelState.Remove("ImageUrl");*/ // Loại bỏ xác thực ModelState cho ImageUrl

            if (id != product.Id)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                var existingProduct = await _productRepository.GetByIdAsync(id); // Giả định có phương thức GetByIdAsync

                // Giữ nguyên thông tin hình ảnh nếu không có hình mới được tải lên
                if (Img_Url == null)
                {
                    product.Img_Url = existingProduct.Img_Url;
                }
                else
                {
                    // Lưu hình ảnh mới
                    product.Img_Url = await SaveImage(Img_Url);
                }
                product.Time = DateTime.Now;
                // Cập nhật các thông tin khác của sản phẩm
                existingProduct.ProductName = product.ProductName;
                existingProduct.Price = product.Price;
                existingProduct.Discount = product.Discount;
                existingProduct.Description = product.Description;
                existingProduct.CategoryId = product.CategoryId;
                existingProduct.Img_Url = product.Img_Url;
                existingProduct.DetailProduct = product.DetailProduct;
                existingProduct.Time = product.Time;
                await _productRepository.UpdateAsync(existingProduct);
                return RedirectToAction(nameof(Index));
            }
            var categories = await _categoryRepository.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            return View(product);
        }
        // Hiển thị form xác nhận xóa sản phẩm
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }
        // Xử lý xóa sản phẩm
        [HttpPost, ActionName("DeleteConfirmed")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _productRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
