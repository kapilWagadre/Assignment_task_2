using Assignment_task_2.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Text.Encodings.Web;
using Microsoft.AspNet.Identity;
using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;

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

        [HttpPost]
        public IActionResult Index(Customer data)
        {
            if (ModelState.IsValid)
            {
                
                var existingCustomer = _context.Customers.FirstOrDefault(c => c.email == data.email);
                if (existingCustomer != null)
                {
                    TempData["ErrorMessage"] = "This email is already registered.";
                    return View(data); 
                }

                
                _context.Customers.Add(data);
                _context.SaveChanges();
                return RedirectToAction("Index1");
            }
            return View(data);
        }

        public IActionResult Index1()
        {
            ClaimsPrincipal claimsUser = HttpContext.User;
            if (claimsUser.Identity.IsAuthenticated)
                return RedirectToAction("HomeIndex", "Add");
            return View();
        }
       // [HttpPost]
        //public IActionResult Index1(string email, string password)
        //{

        //    var user = _context.Customers.FirstOrDefault(u => u.email == email);

        //    if (user != null && (password == user.password))
        //    {

        //        return RedirectToAction("HomeIndex", "Add");
        //    }
        //    else
        //    {
        //        TempData["ErrorMessage"] = "Invalid Email or Password";
        //        return RedirectToAction("Index1", "Home");
        //    }
        //}

        [HttpPost]
        public async Task<IActionResult> Index1(string email, string password)
        {

            var user = await _context.Customers.FirstOrDefaultAsync(u => u.email == email && u.password == password);

            if (user != null)
            {
                List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.email),
            };

                ClaimsIdentity identity = new ClaimsIdentity(claims,
                    CookieAuthenticationDefaults.AuthenticationScheme);
                AuthenticationProperties properties = new AuthenticationProperties()
                {
                    AllowRefresh = true
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(identity), properties);
                var userViewModel = new Customer
                {
                    email = user.email,
                    name = user.name,
                    password = user.password,

                };

                // Use TempData or Session to pass complex objects if necessary
                TempData["UserDetails"] = JsonConvert.SerializeObject(userViewModel);

                return RedirectToAction("HomeIndex", "Add");
            }
        
            else
            {
                TempData["ErrorMessage"] = "User not found !!";
                return RedirectToAction("Index1", "Home");
            }
        }

        public IActionResult ForgetPassword()
        {
            return View();
        }
       

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ForgetPassword(string email)
        {
            if (!string.IsNullOrEmpty(email))
            {
                string resetToken = "random_reset_token";
                string resetLink = Url.Action("ResetPassword", "Home", new { email, token = resetToken }, Request.Scheme);

                string mailBody = $"Please click <a href='{resetLink}'>here</a> to reset your password.";

                MailMessage mailMessage = new MailMessage("kapilwagadre1111@gmail.com", email);
                mailMessage.Subject = "Reset Password";
                mailMessage.Body = mailBody;
                mailMessage.IsBodyHtml = true;

                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);

                smtpClient.Credentials = new NetworkCredential("kapilwagadre1111@gmail.com", "epls lyay lyfc pted");

                smtpClient.EnableSsl = true;
                try
                {
                    smtpClient.Send(mailMessage);
                    return RedirectToAction("Index1");
                }
                catch (Exception ex)
                {
                    return RedirectToAction("ForgetpassError");
                }

            }
            return RedirectToAction("ForgetPassword");
        }


        [HttpGet]
        public ActionResult ResetPassword(string email, string token)
        {
            return View(Tuple.Create(email, token));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(string email, string token, string newPassword, string confirmPassword)
        {

            if (!ModelState.IsValid)
            {
               
                return View();
            }
            var user = _context.Customers.FirstOrDefault(u => u.email == email);
            if (user == null)
            {
                TempData["ErrorMessage"] = "Email not found.";
                return RedirectToAction("ResetPassword", "Home");
            }


            // Update the password
            user.password = newPassword;
            user.cfmpassword = confirmPassword; 
            _context.SaveChanges();
            return RedirectToAction("Index1", "Home");
        }


        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ChangePassword(Changepass model)
        {
            if (ModelState.IsValid)
            {
                var user = _context.Customers.FirstOrDefault(u => u.email == model.Email);

                if (user != null)
                {
                 
                    if (model.CurrentPassword == user.password)
                    {
                        user.password = model.NewPassword;

                        _context.SaveChanges(); 
                        TempData["SuccessMessage"] = "Password changed successfully.";
                        return RedirectToAction("Index1");
                    }
                    else
                    {
                        ModelState.AddModelError("", "The current password is incorrect.");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "User with the given email does not exist.");
                }
            }
            return View(model);
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
