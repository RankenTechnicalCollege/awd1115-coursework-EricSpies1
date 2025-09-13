using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using PersonalFinanceTracker.Data.Repositories;
using PersonalFinanceTracker.Models;

namespace PersonalFinanceTracker.Controllers
{
    [Authorize] // Only logged-in users can access Accounts
    public class AccountsController : Controller
    {
        private readonly IAccountRepository _accounts;
        private readonly UserManager<IdentityUser> _userManager;

        public AccountsController(IAccountRepository accounts, UserManager<IdentityUser> userManager)
        {
            _accounts = accounts;
            _userManager = userManager;
        }

        private string GetUserId()
        {
            return _userManager.GetUserId(User)!;
        }

        // GET: /Accounts
        public async Task<IActionResult> Index()
        {
            var accounts = await _accounts.GetForUserAsync(GetUserId());
            return View(accounts);
        }

        // GET: /Accounts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Accounts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Account account)
        {
            // Ensure UserId is set before validation
            account.UserId = GetUserId();
            ModelState.Remove(nameof(Account.UserId));

            if (!ModelState.IsValid)
            {
                return View(account);
            }

            await _accounts.AddAsync(account);
            return RedirectToAction(nameof(Index));
        }

        // GET: /Accounts/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var acc = await _accounts.GetAsync(id, GetUserId());
            if (acc == null) return NotFound();

            return View(acc);
        }

        // POST: /Accounts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Account account)
        {
            account.UserId = GetUserId();
            ModelState.Remove(nameof(Account.UserId));

            if (!ModelState.IsValid)
            {
                return View(account);
            }

            await _accounts.UpdateAsync(account);
            return RedirectToAction(nameof(Index));
        }

        // GET: /Accounts/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var acc = await _accounts.GetAsync(id, GetUserId());
            if (acc == null) return NotFound();

            return View(acc);
        }

        // POST: /Accounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _accounts.DeleteAsync(id, GetUserId());
            return RedirectToAction(nameof(Index));
        }
    }
}
