using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PersonalFinanceTracker.Data;
using PersonalFinanceTracker.Models;

namespace PersonalFinanceTracker.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class AccountsController : Controller
    {
        private readonly ApplicationDbContext _db;
        public AccountsController(ApplicationDbContext db) => _db = db;

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Account account)
        {
            if (!ModelState.IsValid) return View(account);
            _db.Accounts.Add(account);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index", "Accounts", new { area = "" });
        }

        public async Task<IActionResult> Edit(int id)
        {
            var acct = await _db.Accounts.FindAsync(id);
            if (acct == null) return NotFound();
            return View(acct);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Account posted)
        {
            if (id != posted.Id) return BadRequest();
            if (!ModelState.IsValid) return View(posted);

            _db.Update(posted);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index", "Accounts", new { area = "" });
        }

        public async Task<IActionResult> Delete(int id)
        {
            var acct = await _db.Accounts.FindAsync(id);
            if (acct == null) return NotFound();
            return View(acct);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var acct = await _db.Accounts.FindAsync(id);
            if (acct != null)
            {
                _db.Accounts.Remove(acct);
                await _db.SaveChangesAsync();
            }
            return RedirectToAction("Index", "Accounts", new { area = "" });
        }
    }
}
