using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using TripLog.Data;
using TripLog.Models;
using TripLog.ViewModels;

namespace TripLog.Controllers
{
    public class TripController : Controller
    {
        private readonly AppDbContext _context;

        public TripController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var trips = await _context.Trips
                .Include(t => t.Destination)
                .Include(t => t.Accommodation)
                .Include(t => t.TripActivities)
                    .ThenInclude(ta => ta.Activity)
                .OrderBy(t => t.StartDate)
                .ToListAsync();

            ViewBag.SubHeader = "My Trip Log";
            return View(trips);
        }


        [HttpGet]
        public IActionResult AddPage1()
        {
            var vm = new AddTripPage1VM
            {
                StartDate = System.DateTime.Today,
                EndDate = System.DateTime.Today.AddDays(1)
            };

            LoadDestinationsAndAccommodations(vm);
            ViewBag.SubHeader = "Add Trip Destination and Dates";
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPage1(AddTripPage1VM vm)
        {
            if (!ModelState.IsValid)
            {
                LoadDestinationsAndAccommodations(vm);
                ViewBag.SubHeader = "Add Trip Destination and Dates";
                return View(vm);
            }

            var trip = new Trip
            {
                DestinationId = vm.DestinationId!.Value,
                AccommodationId = vm.AccommodationId!.Value,
                StartDate = vm.StartDate!.Value,
                EndDate = vm.EndDate!.Value
            };

            _context.Trips.Add(trip);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(AddPage2), new { id = trip.TripId });
        }

        private void LoadDestinationsAndAccommodations(AddTripPage1VM vm)
        {
            vm.Destinations = _context.Destinations
                .OrderBy(d => d.Name)
                .Select(d => new SelectListItem
                {
                    Value = d.DestinationId.ToString(),
                    Text = d.Name
                })
                .ToList();

            vm.Accommodations = _context.Accommodations
                .OrderBy(a => a.Name)
                .Select(a => new SelectListItem
                {
                    Value = a.AccommodationId.ToString(),
                    Text = a.Name
                })
                .ToList();
        }


        [HttpGet]
        public async Task<IActionResult> AddPage2(int id)
        {
            var trip = await _context.Trips
                .Include(t => t.Destination)
                .FirstOrDefaultAsync(t => t.TripId == id);

            if (trip == null)
                return RedirectToAction(nameof(Index));

            var vm = new AddTripPage2VM
            {
                TripId = id
            };

            LoadActivities(vm);
            ViewBag.SubHeader = $"Add Things To Do in {trip.Destination?.Name}";
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPage2(AddTripPage2VM vm)
        {
            var trip = await _context.Trips
                .Include(t => t.TripActivities)
                .Include(t => t.Destination)
                .FirstOrDefaultAsync(t => t.TripId == vm.TripId);

            if (trip == null)
                return RedirectToAction(nameof(Index));

            if (!ModelState.IsValid)
            {
                LoadActivities(vm);
                ViewBag.SubHeader = $"Add Things To Do in {trip.Destination?.Name}";
                return View(vm);
            }

            trip.TripActivities.Clear();

            foreach (var actId in vm.SelectedActivityIds)
            {
                trip.TripActivities.Add(new TripActivity
                {
                    TripId = trip.TripId,
                    ActivityId = actId
                });
            }

            await _context.SaveChangesAsync();

            TempData["Message"] = $"Trip to {trip.Destination?.Name} added.";
            return RedirectToAction(nameof(Index));
        }

        private void LoadActivities(AddTripPage2VM vm)
        {
            vm.Activities = _context.Activities
                .OrderBy(a => a.Name)
                .Select(a => new SelectListItem
                {
                    Value = a.ActivityId.ToString(),
                    Text = a.Name
                })
                .ToList();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var trip = await _context.Trips.FindAsync(id);
            if (trip != null)
            {
                _context.Trips.Remove(trip);
                await _context.SaveChangesAsync();
                TempData["Message"] = "Trip deleted.";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Cancel()
        {
            return RedirectToAction(nameof(Index));
        }
    }
}
