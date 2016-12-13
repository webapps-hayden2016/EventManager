using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EventManager.Data;
using Microsoft.AspNetCore.Identity;
using EventManager.Models;
using Microsoft.EntityFrameworkCore;

namespace EventManager.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signinManager;

        public HomeController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signinManager)
        {
            _context = context;
            _userManager = userManager;
            _signinManager = signinManager;
        }

        public IActionResult Index()
        {
            List<Event> eventList = new List<Event>();
            if (_signinManager.IsSignedIn(User))
            {
                var userId = _userManager.GetUserId(User);
                var user = _context.Users.Single(u => u.Id == userId);

                var userevents = from e in _context.UserEvents.Include(e => e.Event)
                                 where e.UserID == userId
                                 select e;

                foreach(var eventx in userevents)
                {
                    if(eventx.Event.Date.CompareTo(DateTime.Now) >= 0 && eventx.Event.Date.CompareTo(DateTime.Now.AddDays(7)) < 0)
                    {
                        eventList.Add(eventx.Event);
                    }
                }

                if (user.ArtistName == null)
                {
                    ViewBag.ArtistName = "";
                }
                else
                {
                    ViewBag.ArtistName = user.ArtistName;
                }
            }
            return View(eventList);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
