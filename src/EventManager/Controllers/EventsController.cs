using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EventManager.Data;
using Microsoft.AspNetCore.Identity;
using EventManager.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace EventManager.Controllers
{
    [Authorize]
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
            var userId = _userManager.GetUserId(User);
            var user = _context.Users.Single(u => u.Id == userId);

            ViewBag.TitleSort = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.ArtistSort = sortOrder == "Artist" ? "artist_desc" : "Artist";
            ViewBag.GenreSort = sortOrder == "Genre" ? "genre_desc" : "Genre";
            ViewBag.DateSort = sortOrder == "Date" ? "date_desc" : "Date";
            ViewBag.TimeSort = sortOrder == "Time" ? "time_desc" : "Time";
            ViewBag.LocationSort = sortOrder == "Location" ? "location_desc" : "Location";

            var events = from e in _context.Events
                         where e.IsActive == true
                         select e;

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

            var userevents = from e in _context.UserEvents.Include(e => e.Event)
                             where e.UserID == userId
                             select e;

            List<Event> eventList = new List<Event>();
            foreach(var eventx in userevents)
            {
                eventList.Add(eventx.Event);
            }
            ViewBag.EventList = eventList;
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

            var eventList = from m in _context.Events
                         select m;

            eventList = eventList.Where(e => e.Artist.Contains(user.ArtistName) && e.IsActive == true);


            switch (sortOrder)
            {
                case "name_desc":
                    eventList = eventList.OrderByDescending(e => e.Name);
                    break;
                case "Artist":
                    eventList = eventList.OrderBy(e => e.Artist);
                    break;
                case "artist_desc":
                    eventList = eventList.OrderByDescending(e => e.Artist);
                    break;
                case "Genre":
                    eventList = eventList.OrderBy(e => e.Genre);
                    break;
                case "genre_desc":
                    eventList = eventList.OrderByDescending(e => e.Genre);
                    break;
                case "Date":
                    eventList = eventList.OrderBy(e => e.Date.ToString("MMMM d, yyyy"));
                    break;
                case "date_desc":
                    eventList = eventList.OrderByDescending(e => e.Date.ToString("MMMM d, yyyy"));
                    break;
                case "Time":
                    eventList = eventList.OrderBy(e => e.Time.ToString("h:mm tt"));
                    break;
                case "time_desc":
                    eventList = eventList.OrderByDescending(e => e.Time.ToString("h:mm tt"));
                    break;
                case "Location":
                    eventList = eventList.OrderBy(e => e.Location);
                    break;
                case "location_desc":
                    eventList = eventList.OrderByDescending(e => e.Location);
                    break;
                default:
                    eventList = eventList.OrderBy(e => e.Name);
                    break;
            }

            return View(eventList.ToList());
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
            anEvent.IsActive = true;
            if (ModelState.IsValid)
            {
                _context.Events.Add(anEvent);
                _context.SaveChanges();
                return RedirectToAction("MyEvents");
            }
            return View(anEvent);
        }

        public IActionResult Remove(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var anEvent = _context.Events.Single(e => e.EventID == id);
                anEvent.IsActive = false;
                _context.Entry(anEvent).State = EntityState.Modified;
                _context.SaveChanges();
            }
            return RedirectToAction("MyEvents"); 
        }
        
        public IActionResult Attend(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);
            var user = _context.Users.Single(u => u.Id == userId);
            var eventx = _context.Events.Single(e => e.EventID == id);
            UserEvent userEvent = new UserEvent();
            userEvent.UserID = userId;
            userEvent.EventID = eventx.EventID;
            _context.UserEvents.Add(userEvent);
            UpdateEvent(eventx, userEvent);
            UpdateUser(user, userEvent);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public void UpdateEvent(Event eventx, UserEvent userEvent)
        {
            eventx.UserEvents.Add(userEvent);
            _context.Entry(eventx).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void UpdateUser(ApplicationUser user, UserEvent userEvent)
        {
            user.UserEvents.Add(userEvent);
            _context.Entry(user).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public IActionResult Attending()
        {
            var userId = _userManager.GetUserId(User);
            var user = _context.Users.Single(u => u.Id == userId);

            var userevents = from e in _context.UserEvents.Include(e => e.Event)
                         where e.UserID == userId
                         select e;
            List<Event> eventList = new List<Event>();

            foreach(var eventx in userevents)
            {
                eventList.Add(eventx.Event);
            }

            return View(eventList);
        }

        public IActionResult Cancel(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var eventx = _context.Events.Single(e => e.EventID == id);
            var userId = _userManager.GetUserId(User);
            var user = _context.Users.Single(u => u.Id == userId);

            var userevent = _context.UserEvents.Single(ue => ue.EventID == eventx.EventID && ue.UserID == userId);
            _context.UserEvents.Remove(userevent);
            UpdateEvent2(eventx, userevent);
            UpdateUser2(user, userevent);
            _context.SaveChanges();
            return RedirectToAction("Attending");
        }

        public void UpdateEvent2(Event eventx, UserEvent userEvent)
        {
            eventx.UserEvents.Remove(userEvent);
            _context.Entry(eventx).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void UpdateUser2(ApplicationUser user, UserEvent userEvent)
        {
            user.UserEvents.Remove(userEvent);
            _context.Entry(user).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public IActionResult Follow(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var eventx = _context.Events.Single(e => e.EventID == id);
            Artist artist = new Artist();
            artist.Name = eventx.Artist;

            var userId = _userManager.GetUserId(User);
            var user = _context.Users.Single(u => u.Id == userId);

            if(user.Following.Exists(f => f == artist) == false)
            {
                user.Following.Add(artist);
            }

            return RedirectToAction("Index");
        }

        public IActionResult Unfollow(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var eventx = _context.Events.Single(e => e.EventID == id);
            Artist artist = new Artist();
            artist.Name = eventx.Artist;

            var userId = _userManager.GetUserId(User);
            var user = _context.Users.Single(u => u.Id == userId);

            if (user.Following.Exists(f => f == artist) == true)
            {
                user.Following.Remove(artist);
            }

            return RedirectToAction("Index");
        }
    }
}
