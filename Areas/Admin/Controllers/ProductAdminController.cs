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
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

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
        private readonly IWebHostEnvironment _env;
        public ProductAdminController(IProductRepository productRepository,

        ICategoryRepository categoryRepository, UserContext context, IWebHostEnvironment env)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _context = context;
            _env = env;
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
        //private async Task<string> SaveImage(IFormFile image)
        //{
        //    var filepath = Path.Combine(_env.WebRootPath, "images", image.FileName);
        //    using var filestream = new FileStream(filepath, FileMode.Create);
        //    await image.CopyToAsync(filestream);

        //    return image.FileName;
        //}
        private async Task<string> SaveImage(IFormFile image)
        {
            return await SaveImageAsync(image);
        }
        private async Task<string> SaveImageAsync(IFormFile image)
        {
            var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images");
            var fileName = Path.GetFileNameWithoutExtension(image.FileName);
            var extension = Path.GetExtension(image.FileName);
            var fullPath = Path.Combine(uploads, image.FileName);

            int count = 1;
            while (System.IO.File.Exists(fullPath))
            {
                fullPath = Path.Combine(uploads, $"{fileName}({count++}){extension}");
            }

            using (var fileStream = new FileStream(fullPath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }
            return Path.GetFileName(fullPath); // Return only the file name
        }

        [HttpPost]
        public async Task<IActionResult> Add(Product product, IFormFile Img_Url, 
            List<IFormFile> Img_Urls)
        {
            if (!ModelState.IsValid)
            {
                if (Img_Url != null)
                {
                    product.Img_Url = await SaveImage(Img_Url);
                }
                if (Img_Urls != null && Img_Urls.Count > 0)
                {
                    product.Img_Urls = await UploadImages(Img_Urls);
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

        //public async Task<List<string>> UploadImages(List<IFormFile> files)
        //{
        //    var imageUrls = new List<string>();

        //    foreach (var image in files)
        //    {
        //        if (image.Length > 0)
        //        {
        //            var filepath = Path.Combine(_env.WebRootPath, "images", image.FileName);
        //            using var filestream = new FileStream(filepath, FileMode.Create);
        //            await image.CopyToAsync(filestream);


        //            imageUrls.Add(image.FileName);
        //        }
        //    }
        //    return imageUrls;

        //}
        private async Task<List<string>> UploadImages(List<IFormFile> images)
        {
            var imagePaths = new List<string>();
            foreach (var image in images)
            {
                var imagePath = await SaveImageAsync(image);
                imagePaths.Add(imagePath);
            }
            return imagePaths;
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
        public async Task<IActionResult> Update(int id, Product product, IFormFile Img_Url,
            List<IFormFile> Img_Urls)
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
                    product.Img_Url = await SaveImage(Img_Url);
                }

                if (Img_Urls == null)
                {
                    product.Img_Urls = existingProduct.Img_Urls;
                }
                else
                {
                    product.Img_Urls = await UploadImages(Img_Urls);
                }
                product.Time = DateTime.Now;
                // Cập nhật các thông tin khác của sản phẩm
                existingProduct.ProductName = product.ProductName;
                existingProduct.Price = product.Price;
                existingProduct.Discount = product.Discount;
                existingProduct.Description = product.Description;
                existingProduct.CategoryId = product.CategoryId;
                existingProduct.Img_Url = product.Img_Url;
                existingProduct.Img_Urls = product.Img_Urls;
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
