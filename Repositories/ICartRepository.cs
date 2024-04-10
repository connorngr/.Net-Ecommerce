﻿using WebApp.Models;

namespace WebApp.Repositories
{
    public interface ICartRepository
    {
        Task<int> AddItem(int ProductId, int qty);
        Task<int> RemoveItem(int ProductId);
        Task<ShoppingCart> GetUserCart();
        Task<int> GetCartItemCount(string userId = "");
        Task<ShoppingCart> GetCart(string userId);
        Task<bool> DoCheckout(string address, string number, string notes);

        Task<int> GetTotalPrice();
    }
}