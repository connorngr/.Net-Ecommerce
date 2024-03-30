using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net;
using WebApp.Areas.Identity.Data;
using WebApp.Data;
using WebApp.Models;

namespace WebApp.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly UserContext _db;
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CartRepository(UserContext db, IHttpContextAccessor httpContextAccessor, UserManager<User> userManager) 
        {
            _db = db;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<int> AddItem(int ProductId, int Qty)
        {
            string userId = GetUserId();
            using var transaction = _db.Database.BeginTransaction();
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
                transaction.Commit();
            }
            catch (Exception ex)
            {
            }
            var cartItemCount = await GetCartItemCount(userId);
            return cartItemCount;
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
        private async Task<ShoppingCart> GetCart(string userId)
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
