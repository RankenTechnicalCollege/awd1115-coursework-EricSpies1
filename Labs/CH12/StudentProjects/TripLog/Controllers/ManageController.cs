using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using TripLog.Data;
using TripLog.Models;

namespace TripLog.Controllers
{
    public class ManageController : Controller
    {
        private readonly AppDbContext _context;

        public ManageController(AppDbContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> Destinations()
        {
            ViewBag.SubHeader = "Manage Destinations";
            var items = await _context.Destinations
                .OrderBy(d => d.Name)
                .ToListAsync();
            return View(items);
        }

        [HttpGet]
        public IActionResult AddDestination()
        {
            ViewBag.SubHeader = "Manage Destinations";
            return View(new Destination());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddDestination(Destination destination)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.SubHeader = "Manage Destinations";
                return View(destination);
            }

            _context.Destinations.Add(destination);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Destinations));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteDestination(int id)
        {
            var dest = await _context.Destinations.FindAsync(id);
            if (dest != null)
            {
                try
                {
                    _context.Destinations.Remove(dest);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException)
                {
                    TempData["Message"] = "Cannot delete a destination that is associated with a trip.";
                }
            }

            return RedirectToAction(nameof(Destinations));
        }


        public async Task<IActionResult> Accommodations()
        {
            ViewBag.SubHeader = "Manage Accommodations";
            var items = await _context.Accommodations
                .OrderBy(a => a.Name)
                .ToListAsync();
            return View(items);
        }

        [HttpGet]
        public IActionResult AddAccommodation()
        {
            ViewBag.SubHeader = "Manage Accommodations";
            return View(new Accommodation());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddAccommodation(Accommodation accommodation)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.SubHeader = "Manage Accommodations";
                return View(accommodation);
            }

            _context.Accommodations.Add(accommodation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Accommodations));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAccommodation(int id)
        {
            var item = await _context.Accommodations.FindAsync(id);
            if (item != null)
            {
                try
                {
                    _context.Accommodations.Remove(item);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException)
                {
                    TempData["Message"] = "Cannot delete an accommodation that is associated with a trip.";
                }
            }

            return RedirectToAction(nameof(Accommodations));
        }


        public async Task<IActionResult> Activities()
        {
            ViewBag.SubHeader = "Manage Activities";
            var items = await _context.Activities
                .OrderBy(a => a.Name)
                .ToListAsync();
            return View(items);
        }

        [HttpGet]
        public IActionResult AddActivity()
        {
            ViewBag.SubHeader = "Manage Activities";
            return View(new Activity());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddActivity(Activity activity)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.SubHeader = "Manage Activities";
                return View(activity);
            }

            _context.Activities.Add(activity);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Activities));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteActivity(int id)
        {
            var item = await _context.Activities.FindAsync(id);
            if (item != null)
            {
                _context.Activities.Remove(item);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Activities));
        }
    }
}
