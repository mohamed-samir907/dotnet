using Clinic_System.PLL.Data;
using Microsoft.AspNetCore.Mvc;

namespace Clinic_System.PLL.Controllers
{
    // Only available in Development environment
    #if DEBUG
    public class SeedController : Controller
    #else
    [ApiExplorerSettings(IgnoreApi = true)]
    public class SeedController : Controller
    #endif
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IWebHostEnvironment _environment;
        private readonly IConfiguration _configuration;

        public SeedController(IServiceProvider serviceProvider, IWebHostEnvironment environment, IConfiguration configuration)
        {
            _serviceProvider = serviceProvider;
            _environment = environment;
            _configuration = configuration;
        }

        /// <summary>
        /// Check if seeding endpoints should be available
        /// </summary>
        private bool IsSeederEnabled()
        {
            // Method 1: Only enable in Development environment
            if (!_environment.IsDevelopment())
                return false;
            
            // Method 2: Also check configuration setting
            return _configuration.GetValue<bool>("SeedingSettings:EnableManualSeeding", false);
        }

        /// <summary>
        /// Manual seeding endpoint - Use this to create default admin user
        /// Navigate to: /Seed/CreateAdmin
        /// </summary>
        [HttpGet]
        public IActionResult CreateAdmin()
        {
            if (!IsSeederEnabled())
            {
                return NotFound("Seeding endpoints are disabled in production.");
            }
            
            return View();
        }

        /// <summary>
        /// POST endpoint to actually create the admin user
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateAdmin(string confirm)
        {
            if (!IsSeederEnabled())
            {
                return NotFound("Seeding endpoints are disabled in production.");
            }
            try
            {
                var result = await SeedData.Initialize(_serviceProvider);
                
                if (result)
                {
                    ViewBag.Success = true;
                    ViewBag.Message = "✅ Admin user seeding completed successfully!";
                    ViewBag.Details = new[]
                    {
                        "📧 Email: admin@clinic.com",
                        "🔑 Password: Admin123!",
                        "⚠️ Please change the default password after first login!"
                    };
                }
                else
                {
                    ViewBag.Success = false;
                    ViewBag.Message = "❌ Admin user seeding failed. Check console for details.";
                }
            }
            catch (Exception ex)
            {
                ViewBag.Success = false;
                ViewBag.Message = $"❌ Error during seeding: {ex.Message}";
            }

            return View();
        }

        /// <summary>
        /// Simple endpoint to check if admin exists
        /// Navigate to: /Seed/CheckAdmin
        /// </summary>
        public async Task<IActionResult> CheckAdmin()
        {
            if (!IsSeederEnabled())
            {
                return NotFound("Seeding endpoints are disabled in production.");
            }
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var userManager = scope.ServiceProvider.GetRequiredService<Microsoft.AspNetCore.Identity.UserManager<Clinic_System.DAL.Entities.User>>();
                    
                    var adminUser = await userManager.FindByEmailAsync("admin@clinic.com");
                    
                    ViewBag.AdminExists = adminUser != null;
                    ViewBag.AdminEmail = adminUser?.Email ?? "Not found";
                    ViewBag.AdminName = adminUser != null ? $"{adminUser.FirstName} {adminUser.LastName}" : "Not found";
                    
                    if (adminUser != null)
                    {
                        var isInRole = await userManager.IsInRoleAsync(adminUser, "Admin");
                        ViewBag.HasAdminRole = isInRole;
                    }
                    else
                    {
                        ViewBag.HasAdminRole = false;
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }

            return View();
        }
    }
}
