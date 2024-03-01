using Assignment_task_2.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace Assignment_task_2.Controllers
{
    public class HomeController : Controller
    {
        private readonly DataBaseConnect _context;
        public HomeController(DataBaseConnect context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
           
            return View();
        }
        public IActionResult Index1()
        {
            ClaimsPrincipal claimsUser = HttpContext.User;
            if (claimsUser.Identity.IsAuthenticated)
                return RedirectToAction("HomeIndex", "Add");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index1(string email, string password)
        {
            var user = _context.Customers.FirstOrDefault(u => u.email == email && u.password == password);

            if (user != null)
            {
                List<Claim> claims = new List<Claim>()
            {
              new Claim(ClaimTypes.NameIdentifier, user.email)
            };

                ClaimsIdentity identity = new ClaimsIdentity(claims,
                    CookieAuthenticationDefaults.AuthenticationScheme);
                AuthenticationProperties properties = new AuthenticationProperties()
                {
                    AllowRefresh = true
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(identity), properties);

                return RedirectToAction("HomeIndex", "Add");
            }

            else
            {
                TempData["ErrorMessage"] = "Invalid Email or Password";
                return View();
            }
        }


        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index1");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
