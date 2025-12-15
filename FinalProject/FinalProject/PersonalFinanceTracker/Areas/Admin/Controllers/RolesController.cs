using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace PersonalFinanceTracker.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RolesController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        [HttpGet("admin/roles/")]
        public IActionResult Index()
        {
            var roles = _roleManager.Roles
                .OrderBy(r => r.Name)
                .ToList();

            return View(roles);
        }

        [HttpPost("admin/roles/create/")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
            {
                TempData["Message"] = "Role name is required.";
                return RedirectToAction(nameof(Index));
            }

            roleName = roleName.Trim();

            if (await _roleManager.RoleExistsAsync(roleName))
            {
                TempData["Message"] = "That role already exists.";
                return RedirectToAction(nameof(Index));
            }

            await _roleManager.CreateAsync(new IdentityRole(roleName));
            TempData["Message"] = $"Role \"{roleName}\" created.";
            return RedirectToAction(nameof(Index));
        }
    }
}
