using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

using WebShopApp.Infrastructure.Data.Domain;

namespace WebShopApp.Infrastructure.Data.Infrastructure
{
    public static class ApplicationBuilderExtention
    {
        public static async Task<IApplicationBuilder> PrepareDatabase(this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            var services = serviceScope.ServiceProvider;

            await RoleSeeder(services);
            await SeedAdimistrator(services);


            var dataCategory = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            SeedCategories(dataCategory);


            var dataBrand = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            SeedBrands(dataBrand);
            return app;
        }


        public static void SeedCategories(ApplicationDbContext dataCategory)
        {
            if (dataCategory.Categories.Any())
            {
                return;
            }
            dataCategory.Categories.AddRange(new[]
            {
                new Category {CategoryName = "Lapton"},
                new Category {CategoryName = "Computer"},
                new Category {CategoryName = "Monitor"},
                new Category {CategoryName = "Accessory"},
                new Category {CategoryName = "TV"},
                new Category {CategoryName = "Mobile phone"},
                new Category {CategoryName = "Smart watch"},
            });
            dataCategory.SaveChanges();
        }


        public static void SeedBrands(ApplicationDbContext dataBrand)
        {
            if (dataBrand.Brands.Any())
            {
                return;
            }
            dataBrand.Brands.AddRange(new[]
            {
                new Brand {BrandName = "Acer"},
                new Brand {BrandName = "Asus"},
                new Brand {BrandName = "Apple"},
                new Brand {BrandName = "Dell"},
                new Brand {BrandName = "HP"},
                new Brand {BrandName = "Huawei"},
                new Brand {BrandName = "Lenovo"},
                new Brand {BrandName = "Samsung"},
            });
            dataBrand.SaveChanges();
        }


        private static async Task RoleSeeder(IServiceProvider servicepProvider)
        {
            var roleManager = servicepProvider.GetRequiredService<RoleManager<IdentityRole>>();
            string[] roleNames = { "Administrator", "Client" };
            IdentityResult roleResult;
            foreach (var role in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(role);
                if (!roleExist)
                {
                    roleResult = await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }


        private static async Task SeedAdimistrator(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            if (await userManager.FindByNameAsync("admin") == null)
            {
                ApplicationUser user = new ApplicationUser();
                user.FirstName = "admin";
                user.LastName = "admin";
                user.UserName = "admin";
                user.Email = "admin@admin.com";
                user.Address = "admin";
                user.PhoneNumber = "0895519837";

                var result = await userManager.CreateAsync(user, "123123");
                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Administrator").Wait();
                }

            }
        }
    }
}
