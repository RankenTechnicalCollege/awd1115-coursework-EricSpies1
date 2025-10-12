using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using TripLog.Data;
using TripLog.Infrastructure;
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
            ViewBag.SubHeader = "";
            var trips = await _context.Trips.OrderBy(t => t.StartDate).ToListAsync();
            return View(trips);
        }

        [HttpGet]
        public IActionResult AddPage1()
        {
            ViewBag.SubHeader = "Add Trip Destination and Dates";
            return View(new AddTripPage1VM());
        }

        [HttpPost]
        public IActionResult AddPage1(AddTripPage1VM vm)
        {
            ViewBag.SubHeader = "Add Trip Destination and Dates";
            if (!ModelState.IsValid) return View(vm);

            TempData.Put("p1", vm);
            return RedirectToAction(nameof(AddPage2));
        }

        [HttpGet]
        public IActionResult AddPage2()
        {
            var p1 = TempData.Get<AddTripPage1VM>("p1");
            if (p1 is null) return RedirectToAction(nameof(Index));

            TempData.Put("p1", p1);
            ViewBag.SubHeader = $"Add Info for {p1.Accommodation}";
            return View(new AddTripPage2VM());
        }

        [HttpPost]
        public IActionResult AddPage2(AddTripPage2VM vm)
        {
            var p1 = TempData.Get<AddTripPage1VM>("p1");
            if (p1 is null) return RedirectToAction(nameof(Index));

            TempData.Put("p1", p1);
            TempData.Put("p2", vm);

            if (!ModelState.IsValid)
            {
                ViewBag.SubHeader = $"Add Info for {p1.Accommodation}";
                return View(vm);
            }

            return RedirectToAction(nameof(AddPage3));
        }

        [HttpGet]
        public IActionResult AddPage3()
        {
            var p1 = TempData.Get<AddTripPage1VM>("p1");
            var p2 = TempData.Get<AddTripPage2VM>("p2");
            if (p1 is null) return RedirectToAction(nameof(Index));

            TempData.Put("p1", p1);
            if (p2 != null) TempData.Put("p2", p2);

            ViewBag.SubHeader = $"Add Things To Do in {p1.Destination}";
            return View(new AddTripPage3VM());
        }

        [HttpPost]
        public async Task<IActionResult> AddPage3(AddTripPage3VM vm)
        {
            var p1 = TempData.Get<AddTripPage1VM>("p1");
            var p2 = TempData.Get<AddTripPage2VM>("p2");
            if (p1 is null) return RedirectToAction(nameof(Index));

            var trip = new Trip
            {
                Destination = p1.Destination,
                Accommodation = p1.Accommodation,
                StartDate = p1.StartDate!.Value,
                EndDate = p1.EndDate!.Value,
                AccommodationPhone = p2?.AccommodationPhone,
                AccommodationEmail = p2?.AccommodationEmail,
                Activity1 = vm.Activity1,
                Activity2 = vm.Activity2,
                Activity3 = vm.Activity3
            };

            _context.Trips.Add(trip);
            await _context.SaveChangesAsync();

            TempData.Clear();
            TempData["Message"] = $"Trip to {trip.Destination} added.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Cancel()
        {
            TempData.Clear();
            return RedirectToAction(nameof(Index));
        }
    }
}
