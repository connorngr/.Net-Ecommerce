using Microsoft.AspNetCore.Identity;
using WebApp.Constants;

namespace WebApp.Areas.Identity.Data
{
    public class DbSeeder
    {
        public static async Task SeedDefaultData(IServiceProvider service)
        {
            var userMgr = service.GetService<UserManager<ApplicationUser>>();
            var roleMgr = service.GetService<RoleManager<IdentityRole>>();
            //adding some roles to db
            await roleMgr.CreateAsync(new IdentityRole("Admin"));
            await roleMgr.CreateAsync(new IdentityRole("Employee"));
            await roleMgr.CreateAsync(new IdentityRole("Customer"));

            var admin = new ApplicationUser
            {
                UserName = "hani101003@gmail.com",
                Email = "hani101003@gmail.com",
                PhoneNumber = "0981020042",
                FullName = "Lộc",
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
