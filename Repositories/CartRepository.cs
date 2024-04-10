using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System.Net;
using WebApp.Areas.Identity.Data;
using WebApp.Data;
using WebApp.Models;
using WebApp.Services;

namespace WebApp.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly UserContext _db;
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger _logger;
        public CartRepository(UserContext db, IHttpContextAccessor httpContextAccessor, UserManager<User> userManager, ILogger<CartRepository> logger) 
        {
            _db = db;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }
        public async Task<int> AddItem(int ProductId, int Qty)
        {
            string userId = GetUserId();
            /*using var transaction = _db.Database.BeginTransaction();*/
            try
            {
                if (string.IsNullOrEmpty(userId))
                    throw new Exception("user is not logged-in");
                var cart = await GetCart(userId);
                if (cart is null)
                {
                    cart = new ShoppingCart
                    {
                        UserId = userId
                    };
                    _db.ShoppingCarts.Add(cart);
                }
                _db.SaveChanges();
                // cart detail section
                var cartItem = _db.CartDetails
                                  .FirstOrDefault(a => a.ShoppingCartId == cart.Id && a.ProductId == ProductId);
                if (cartItem is not null)
                {
                    cartItem.Quantity += Qty;
                }
                else
                {
                    var product = _db.Products.Find(ProductId);
                    cartItem = new CartDetail
                    {
                        ProductId = ProductId,
                        ShoppingCartId = cart.Id,
                        Quantity = Qty,
                        UnitPriced = product.Price  // it is a new line after update
                    };
                    _db.CartDetails.Add(cartItem);
                }
                _db.SaveChanges();
                /*transaction.Commit();*/
            }
            catch (Exception ex)
            {
            }
            var cartItemCount = await GetCartItemCount(userId);
            return cartItemCount;
        }
        public async Task<int> RemoveItem(int ProductId)
        {
            //using var transaction = _db.Database.BeginTransaction();
            string userId = GetUserId();
            try
            {
                if (string.IsNullOrEmpty(userId))
                    throw new Exception("user is not logged-in");
                var cart = await GetCart(userId);
                if (cart is null)
                    throw new Exception("Invalid cart");
                // cart detail section
                var cartItem = _db.CartDetails
                                  .FirstOrDefault(a => a.ShoppingCartId == cart.Id && a.ProductId == ProductId);
                if (cartItem is null)
                    throw new Exception("No items in cart");
                else if (cartItem.Quantity == 1)
                    _db.CartDetails.Remove(cartItem);
                else
                    cartItem.Quantity = cartItem.Quantity - 1;
                _db.SaveChanges();
            }
            catch (Exception ex)
            {

            }
            var cartItemCount = await GetCartItemCount(userId);
            return cartItemCount;
        }

        public async Task<int> GetTotalPrice()
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
                throw new Exception("User is not logged-in");
            var cart = await GetCart(userId);
            if (cart is null)
                throw new Exception("Invalid cart");
            var cartDetail = _db.CartDetails
                                .Where(a => a.ShoppingCartId == cart.Id).ToList();
            if (cartDetail.Count == 0)
                throw new Exception("Cart is empty");
            var totalAmount = 0;
            foreach (var cartItem in cartDetail)
            {
                totalAmount += cartItem.UnitPriced * cartItem.Quantity;
            }
            return totalAmount;
        }
        public async Task<bool> DoCheckout(string address, string number, string notes)
        {
            using var transaction = _db.Database.BeginTransaction();
            try
            {
                // logic
                // move data from cartDetail to order and order detail then we will remove cart detail
                var userId = GetUserId();
                if (string.IsNullOrEmpty(userId))
                    throw new Exception("User is not logged-in");
                var cart = await GetCart(userId);
                if (cart is null)
                    throw new Exception("Invalid cart");
                var cartDetail = _db.CartDetails
                                    .Where(a => a.ShoppingCartId == cart.Id).ToList();
                if (cartDetail.Count == 0)
                    throw new Exception("Cart is empty");
                var totalAmount = 0;
                foreach (var cartItem in cartDetail)
                {
                    totalAmount += cartItem.UnitPriced * cartItem.Quantity;
                }
                _logger.LogInformation(totalAmount.ToString());
                var order = new Order
                {
                    UserId = userId,
                    OrderDate = DateTime.UtcNow,
                    OrderStatusId = 1,//pending
                    Notes = notes,
                    PhoneNumber = number,
                    ShippingAddress = address
                };
                _db.Orders.Add(order);
                _db.SaveChanges();
                foreach (var item in cartDetail)
                {
                    var orderDetail = new OrderDetail
                    {
                        ProductId = item.ProductId,
                        OrderId = order.Id,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPriced
                    };
                    _db.OrderDetails.Add(orderDetail);
                }
                _db.SaveChanges();

                // removing the cartdetails
                _db.CartDetails.RemoveRange(cartDetail);
                _db.SaveChanges();
                transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return false;
            }
        }
        
        public async Task<ShoppingCart> GetUserCart()
        {
            var userId = GetUserId();
            if (userId == null)
                throw new Exception("Invalid userid");
            var shoppingCart = await _db.ShoppingCarts
                                  .Include(a => a.CartDetails)
                                  .ThenInclude(a => a.Product)
                                  .ThenInclude(a => a.Category)
                                  .Where(a => a.UserId == userId).FirstOrDefaultAsync();
            return shoppingCart;
        }
        public async Task<int> GetCartItemCount(string userId = "")
        {
            if (string.IsNullOrEmpty(userId)) // updated line
            {
                userId = GetUserId();
            }
            var data = await (from cart in _db.ShoppingCarts
                              join cartDetail in _db.CartDetails
                              on cart.Id equals cartDetail.ShoppingCartId
                              where cart.UserId == userId // updated line
                              select new { cartDetail.Id }
                        ).ToListAsync();
            return data.Count;
        }
        public async Task<ShoppingCart> GetCart(string userId)
        {
            var cart = _db.ShoppingCarts.FirstOrDefault(x => x.UserId == userId);
            return cart;
        }
        private string GetUserId()
        {
            var user = _httpContextAccessor.HttpContext.User;
            var userId = _userManager.GetUserId(user);
            return userId;
        }
    }
}
