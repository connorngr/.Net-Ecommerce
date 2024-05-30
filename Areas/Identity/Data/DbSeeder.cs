using Innerglow_App.Models;
using Microsoft.AspNetCore.Identity;

namespace Innerglow_App.Areas.Identity.Data
{
    public class DbSeeder
    {
        public static async Task SeedDefaultData(IServiceProvider service)
        {
            var userMgr = service.GetService<UserManager<User>>();
            var roleMgr = service.GetService<RoleManager<IdentityRole>>();
            var context = service.GetService<UserContext>();
            var orderStatus = new List<OrderStatus>
            {
                new OrderStatus {StatusId = 1, StatusName = "Pending"},
                new OrderStatus {StatusId = 2, StatusName = "Shipping"},
                new OrderStatus {StatusId = 3, StatusName = "Delivered"}
            };

            if (context?.OrderStatus.Count() == 0)
            {
                context?.OrderStatus.AddRange(orderStatus);
                context?.SaveChanges();
            }
            //adding some roles to db
            await roleMgr.CreateAsync(new IdentityRole("Admin"));
            await roleMgr.CreateAsync(new IdentityRole("Employee"));
            await roleMgr.CreateAsync(new IdentityRole("Customer"));

            var admin = new User
            {
                UserName = "admin123@gmail.com",
                Email = "admin123@gmail.com",
                FullName = "Admin",
                PhoneNumber = "0981020042",
                LockoutEnabled = false,
                EmailConfirmed = true

            };

            var userInDb = await userMgr.FindByEmailAsync(admin.Email);
            if (userInDb is null)
            {
                await userMgr.CreateAsync(admin, "Abc@123");
                await userMgr.AddToRoleAsync(admin, "Admin");
            }

        }
    }
}
