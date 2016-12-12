using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EventManager.Data;
using Microsoft.AspNetCore.Identity;
using EventManager.Models;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace EventManager.Controllers
{
    public class EventsController : Controller
    {
        public readonly ApplicationDbContext _context;
        public readonly UserManager<ApplicationUser> _userManager;

        public EventsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: /<controller>/
        public IActionResult Index(string searchString, string sortOrder)
        {
            ViewBag.TitleSort = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.ArtistSort = sortOrder == "Artist" ? "artist_desc" : "Artist";
            ViewBag.GenreSort = sortOrder == "Genre" ? "genre_desc" : "Genre";
            ViewBag.DateSort = sortOrder == "Date" ? "date_desc" : "Date";
            ViewBag.TimeSort = sortOrder == "Time" ? "time_desc" : "Time";
            ViewBag.LocationSort = sortOrder == "Location" ? "location_desc" : "Location";

            var events = from m in _context.Events
                         select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                events = events.Where(e => e.Name.Contains(searchString) || e.Artist.Contains(searchString) || e.Genre.Contains(searchString) || e.Location.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    events = events.OrderByDescending(e => e.Name);
                    break;
                case "Artist":
                    events = events.OrderBy(e => e.Artist);
                    break;
                case "artist_desc":
                    events = events.OrderByDescending(e => e.Artist);
                    break;
                case "Genre":
                    events = events.OrderBy(e => e.Genre);
                    break;
                case "genre_desc":
                    events = events.OrderByDescending(e => e.Genre);
                    break;
                case "Date":
                    events = events.OrderBy(e => e.Date);
                    break;
                case "date_desc":
                    events = events.OrderByDescending(e => e.Date);
                    break;
                case "Time":
                    events = events.OrderBy(e => e.Time);
                    break;
                case "time_desc":
                    events = events.OrderByDescending(e => e.Time);
                    break;
                case "Location":
                    events = events.OrderBy(e => e.Location);
                    break;
                case "location_desc":
                    events = events.OrderByDescending(e => e.Location);
                    break;
                default:
                    events = events.OrderBy(e => e.Name);
                    break;
            }

            return View(events.ToList());
        }

        public IActionResult MyEvents(string sortOrder)
        {
            var userId = _userManager.GetUserId(User);
            var user = _context.Users.Single(u => u.Id == userId);

            ViewBag.ArtistName = user.ArtistName;

            ViewBag.TitleSort = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.ArtistSort = sortOrder == "Artist" ? "artist_desc" : "Artist";
            ViewBag.GenreSort = sortOrder == "Genre" ? "genre_desc" : "Genre";
            ViewBag.DateSort = sortOrder == "Date" ? "date_desc" : "Date";
            ViewBag.TimeSort = sortOrder == "Time" ? "time_desc" : "Time";
            ViewBag.LocationSort = sortOrder == "Location" ? "location_desc" : "Location";

            var events = from m in _context.Events
                         select m;

            events = events.Where(e => e.Artist.Contains(user.ArtistName));


            switch (sortOrder)
            {
                case "name_desc":
                    events = events.OrderByDescending(e => e.Name);
                    break;
                case "Artist":
                    events = events.OrderBy(e => e.Artist);
                    break;
                case "artist_desc":
                    events = events.OrderByDescending(e => e.Artist);
                    break;
                case "Genre":
                    events = events.OrderBy(e => e.Genre);
                    break;
                case "genre_desc":
                    events = events.OrderByDescending(e => e.Genre);
                    break;
                case "Date":
                    events = events.OrderBy(e => e.Date.ToString("MMMM d, yyyy"));
                    break;
                case "date_desc":
                    events = events.OrderByDescending(e => e.Date.ToString("MMMM d, yyyy"));
                    break;
                case "Time":
                    events = events.OrderBy(e => e.Time.ToString("h:mm tt"));
                    break;
                case "time_desc":
                    events = events.OrderByDescending(e => e.Time.ToString("h:mm tt"));
                    break;
                case "Location":
                    events = events.OrderBy(e => e.Location);
                    break;
                case "location_desc":
                    events = events.OrderByDescending(e => e.Location);
                    break;
                default:
                    events = events.OrderBy(e => e.Name);
                    break;
            }

            return View(events.ToList());
        }

        public IActionResult Create()
        {
            var userId = _userManager.GetUserId(User);
            var user = _context.Users.Single(u => u.Id == userId);

            ViewBag.ArtistName = user.ArtistName;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Event anEvent)
        {
            if (ModelState.IsValid)
            {
                _context.Events.Add(anEvent);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(anEvent);
        }
    }
}
