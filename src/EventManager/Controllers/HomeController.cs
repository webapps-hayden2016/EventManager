using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EventManager.Data;
using Microsoft.AspNetCore.Identity;
using EventManager.Models;

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
            if (_signinManager.IsSignedIn(User))
            {
                var userId = _userManager.GetUserId(User);
                var user = _context.Users.Single(u => u.Id == userId);

                if (user.ArtistName == null)
                {
                    ViewBag.ArtistName = "";
                }
                else
                {
                    ViewBag.ArtistName = user.ArtistName;
                }
            }
            return View();
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
