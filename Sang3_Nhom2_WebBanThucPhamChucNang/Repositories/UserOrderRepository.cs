﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Sang3_Nhom2_WebBanThucPhamChucNang.Areas.Identity.Data;
using Sang3_Nhom2_WebBanThucPhamChucNang.Data;
using Sang3_Nhom2_WebBanThucPhamChucNang.Models;

namespace Sang3_Nhom2_WebBanThucPhamChucNang.Repositories
{
    public class UserOrderRepository : IUserOrderRepository
    {
        private readonly UserContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<User> _userManager;


        public UserOrderRepository(UserContext db,
            UserManager<User> userManager,
             IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }
        public async Task<IEnumerable<Order>> UserOrders()
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
                throw new Exception("User is not logged-in");
            IEnumerable<Order> orders = await _db.Orders
                            .Include(x => x.OrderStatus)
                            .Include(x => x.OrderDetail)
                            .ThenInclude(x => x.Product)
                            .ThenInclude(x => x.Category)
                            .Where(a => a.UserId == userId)
                            .ToListAsync();
            return orders;
        }

        private string GetUserId()
        {
            var principal = _httpContextAccessor.HttpContext.User;
            string userId = _userManager.GetUserId(principal);
            return userId;
        }
    }
}
