﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Sang3_Nhom2_WebBanThucPhamChucNang.Areas.Identity.Data;
using Sang3_Nhom2_WebBanThucPhamChucNang.Models;

namespace Sang3_Nhom2_WebBanThucPhamChucNang.Data;

public class UserContext : IdentityDbContext<User>
{
    public UserContext(DbContextOptions<UserContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);

    }


    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderDetail> OrderDetails { get; set; }
    public DbSet<OrderStatus> OrderStatus { get; set; }
    public DbSet<ShoppingCart> ShoppingCarts { get; set; }
    public DbSet<CartDetail> CartDetails { get; set; }

}