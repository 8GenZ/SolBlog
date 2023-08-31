using Microsoft.AspNetCore.Identity;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using SolBlog.Models;

namespace SolBlog.Data
{
    public static class DataUtility
    {
        private const string? _adminRole = "Admin";
        private const string? _moderatorRole = "Moderator";
        public static string GetConnectionString(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
            return string.IsNullOrEmpty(databaseUrl) ? connectionString! : BuildConnectionString(databaseUrl);
        }

        private static string BuildConnectionString(string databaseUrl)
        {
            //Provides an object representation of a uniform resource identifier (URI) and easy access to the parts of the URI.
            var databaseUri = new Uri(databaseUrl);
            var userInfo = databaseUri.UserInfo.Split(':');
            //Provides a simple way to create and manage the contents of connection strings used by the NpgsqlConnection class.
            var builder = new NpgsqlConnectionStringBuilder
            {
                Host = databaseUri.Host,
                Port = databaseUri.Port,
                Username = userInfo[0],
                Password = userInfo[1],
                Database = databaseUri.LocalPath.TrimStart('/'),
                SslMode = SslMode.Prefer,
                TrustServerCertificate = true
            };
            return builder.ToString();
        }

        public static async Task ManageDataAsync(IServiceProvider svcProvider)
        {

            var dbContextSvc = svcProvider.GetRequiredService<ApplicationDbContext>();
            var userManagerSvc = svcProvider.GetRequiredService<UserManager<BlogUser>>();
            var configurationSvc = svcProvider.GetRequiredService<IConfiguration>();
            var roleManagerSvc = svcProvider.GetRequiredService<RoleManager<IdentityRole>>();

            //Align the database by checking Migrations
            await dbContextSvc.Database.MigrateAsync();

            //Seed app roles
            await SeedRolesAsync(roleManagerSvc);

            //Seed Users
            await SeedBlogUsersAsync(userManagerSvc, configurationSvc);

        }

        private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            if (!await roleManager.RoleExistsAsync(_adminRole!))
            {
                await roleManager.CreateAsync(new IdentityRole(_adminRole!));
            }
            if (!await roleManager.RoleExistsAsync(_moderatorRole!))
            {
                await roleManager.CreateAsync(new IdentityRole(_moderatorRole!));
            }
        }

        private static async Task SeedBlogUsersAsync(UserManager<BlogUser> userManager, IConfiguration configuration)
        {
            string? adminEmail = configuration["AdminLoginEmail"] ?? Environment.GetEnvironmentVariable("AdminLoginEmail");
            string? adminPassword = configuration["AdminPwd"] ?? Environment.GetEnvironmentVariable("AdminPwd");

            string? moderatorEmail = configuration["ModeratorLoginEmail"] ?? Environment.GetEnvironmentVariable("ModeratorLoginEmail");
            string? moderatorPassword = configuration["ModeratorPwd"] ?? Environment.GetEnvironmentVariable("ModeratorPwd");

            //seed the admin
            BlogUser adminUser = new BlogUser()
                {

                    UserName = adminEmail,
                    Email = adminEmail,
                    FirstName = "Admin",
                    LastName = "Sol",
                    EmailConfirmed = true
                };
            try
            {
                BlogUser? blogUser = await userManager.FindByEmailAsync(adminEmail!);

                if (blogUser == null)
                {
                    await userManager.CreateAsync(adminUser, adminPassword!);
                    await userManager.AddToRoleAsync(adminUser, _adminRole!);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("********** ERROR **********");
                Console.WriteLine("Error Seeding Admin!!");
                Console.WriteLine(ex.Message);
                Console.WriteLine("***************************");
                throw;
            }

            //seed the moderator
            BlogUser moderatorUser = new BlogUser()
            {

                UserName = moderatorEmail,
                Email = moderatorEmail,
                FirstName = "Moderator",
                LastName = "Sol",
                EmailConfirmed = true
            };
            try
            {
                BlogUser? blogUser = await userManager.FindByEmailAsync(moderatorEmail!);

                if (blogUser == null)
                {
                    await userManager.CreateAsync(moderatorUser, moderatorPassword!);
                    await userManager.AddToRoleAsync(moderatorUser, _moderatorRole!);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("********** ERROR **********");
                Console.WriteLine("Error Seeding Moderator!");
                Console.WriteLine(ex.Message);
                Console.WriteLine("***************************");
                throw;
            }
        }

    }
}
