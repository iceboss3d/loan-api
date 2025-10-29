using Loan.Api.Data.Contexts;
using Loan.Api.Models;
using Microsoft.AspNetCore.Identity;

namespace Loan.Api.Data.Seeders;

public class Seeder
{
    public static async Task AdminSeeder(IApplicationBuilder application)
    {
        using var serviceScope = application.ApplicationServices.CreateScope();
        await Seed((RoleManager<IdentityRole>)serviceScope.ServiceProvider.GetService(typeof(RoleManager<IdentityRole>)),
            (UserManager<AppUser>)serviceScope.ServiceProvider.GetService(typeof(UserManager<AppUser>)),
            serviceScope.ServiceProvider.GetService<AppDbContext>());
    }
    public static async Task Seed(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager, AppDbContext dbContext)
    {
        await dbContext.Database.EnsureCreatedAsync();
        if (!dbContext.Users.Any())
        {
            List<string> roles = ["SuperAdmin", "Admin", "Instructor", "Student"];
            foreach (var role in roles)
            {
                await roleManager.CreateAsync(new IdentityRole { Name = role });
            }

            AppUser superUser = new()
            {
                FirstName = "Ayebakuro",
                LastName = "Ombu",
                Email = "xplicitkuro@gmail.com",
                UserName = "xplicitkuro@gmail.com",
                EmailConfirmed = true,
            };

            await userManager.CreateAsync(superUser, "NoReply@1");
            await userManager.AddToRolesAsync(superUser, ["SuperAdmin", "Admin", "Instructor", "Student"]);



            await dbContext.SaveChangesAsync();
        }
    }
}