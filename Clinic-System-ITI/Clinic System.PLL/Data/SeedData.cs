using Clinic_System.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Clinic_System.PLL.Data
{
    public static class SeedData
    {
        public static async Task<bool> Initialize(IServiceProvider serviceProvider)
        {
            try
            {
                using (var scope = serviceProvider.CreateScope())
                {
                    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

                    Console.WriteLine("🔄 Starting admin user seeding process...");
                    
                    var rolesResult = await SeedRoles(roleManager);
                    var adminResult = await SeedDefaultAdmin(userManager);
                    
                    Console.WriteLine("✅ Admin user seeding completed successfully!");
                    return rolesResult && adminResult;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error during seeding: {ex.Message}");
                return false;
            }
        }

        private static async Task<bool> SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            try
            {
                // Create roles if they don't exist
                string[] roles = { "Admin", "Doctor", "Patient" };

                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        var result = await roleManager.CreateAsync(new IdentityRole(role));
                        if (result.Succeeded)
                        {
                            Console.WriteLine($"✅ Created role: {role}");
                        }
                        else
                        {
                            Console.WriteLine($"❌ Failed to create role {role}:");
                            foreach (var error in result.Errors)
                            {
                                Console.WriteLine($"   - {error.Description}");
                            }
                            return false;
                        }
                    }
                    else
                    {
                        Console.WriteLine($"ℹ️  Role already exists: {role}");
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error seeding roles: {ex.Message}");
                return false;
            }
        }

        private static async Task<bool> SeedDefaultAdmin(UserManager<User> userManager)
        {
            try
            {
                // Check if admin user already exists
                var adminUser = await userManager.FindByEmailAsync("admin@clinic.com");

                if (adminUser == null)
                {
                    Console.WriteLine("🔄 Creating default admin user...");
                    
                    // Create default admin user
                    adminUser = new User
                    {
                        UserName = "admin@clinic.com",
                        Email = "admin@clinic.com",
                        EmailConfirmed = true,
                        FirstName = "System",
                        LastName = "Administrator",
                        Age = 35,
                        Gender = DAL.Enum.Gender.Male,
                        IsDeleted = false
                    };

                    var result = await userManager.CreateAsync(adminUser, "Admin123!");

                    if (result.Succeeded)
                    {
                        // Assign Admin role
                        var roleResult = await userManager.AddToRoleAsync(adminUser, "Admin");
                        
                        if (roleResult.Succeeded)
                        {
                            // Add admin claims for additional security
                            var claims = new List<Claim>
                            {
                                new Claim("AdminLevel", "SystemAdmin"),
                                new Claim("FullName", $"{adminUser.FirstName} {adminUser.LastName}"),
                                new Claim("CreatedAt", DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"))
                            };
                            
                            await userManager.AddClaimsAsync(adminUser, claims);
                            
                            Console.WriteLine("✅ Default admin user created successfully!");
                            Console.WriteLine("📧 Email: admin@clinic.com");
                            Console.WriteLine("🔑 Password: Admin123!");
                            Console.WriteLine("⚠️  Please change the default password after first login!");
                            return true;
                        }
                        else
                        {
                            Console.WriteLine("❌ Failed to assign Admin role:");
                            foreach (var error in roleResult.Errors)
                            {
                                Console.WriteLine($"   - {error.Description}");
                            }
                            return false;
                        }
                    }
                    else
                    {
                        Console.WriteLine("❌ Failed to create admin user:");
                        foreach (var error in result.Errors)
                        {
                            Console.WriteLine($"   - {error.Description}");
                        }
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("ℹ️  Admin user already exists: admin@clinic.com");
                    // Make sure existing admin has the Admin role
                    if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
                    {
                        var roleResult = await userManager.AddToRoleAsync(adminUser, "Admin");
                        if (roleResult.Succeeded)
                        {
                            Console.WriteLine("✅ Admin role assigned to existing user");
                        }
                        else
                        {
                            Console.WriteLine("❌ Failed to assign Admin role to existing user");
                            return false;
                        }
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error seeding admin user: {ex.Message}");
                return false;
            }
        }
    }
}
