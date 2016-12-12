using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EventManager.Data;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace EventManager.Controllers
{
    public class EventsController : Controller
    {
        public readonly ApplicationDbContext _context;

        public EventsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /<controller>/
        public IActionResult Index(string searchString, string sortOrder)
        {
            ViewBag.TitleSort = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.ArtistSort = sortOrder == "Artist" ? "artist_desc" : "Artist";
            ViewBag.GenreSort = sortOrder == "Genre" ? "genre_desc" : "Genre";
            ViewBag.DateTimeSort = sortOrder == "DateTime" ? "datetime_desc" : "DateTime";
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
                case "DateTime":
                    events = events.OrderBy(e => e.Date);
                    break;
                case "datetime_desc":
                    events = events.OrderByDescending(e => e.Date);
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
    }
}
