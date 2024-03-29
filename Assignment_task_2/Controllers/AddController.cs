﻿using Assignment_task_2.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.AspNetCore.Authorization;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Newtonsoft.Json;

namespace Assignment_task_2.Controllers
{
   [Authorize]
    public class AddController : Controller
    {
        private readonly DataBaseConnect _context;
        public AddController(DataBaseConnect context)
        {
            _context = context;
        }
        public IActionResult HomeIndex()
        {
            if (TempData["UserDetails"] is string userDetailsJson)
            {
                var userDetails = JsonConvert.DeserializeObject<Customer>(userDetailsJson);
                return View(userDetails); 
            }

            return View();
        }




        //public IActionResult SignIn(string email, string password)
        //{

        //    var user = _context.Customers.FirstOrDefault(u => u.email == email);

        //    if (user != null && (password == user.password))
        //    {

        //        return RedirectToAction("HomeIndex");
        //    }
        //    else
        //    {
        //        TempData["ErrorMessage"] = "Invalid Email or Password";
        //        return RedirectToAction("Index1", "Home");
        //    }

        //}

        //public IActionResult HomeIndex()
        //{
        //    return View();
        //}

        public IActionResult HomePage()
        {
            return View();
        }

        public IActionResult HomeChangePass(string email)
        {
            var user = _context.Customers.FirstOrDefault(u => u.email == email);

            if (user != null)
            {
                var model = new Changepass
                {
                    Email = user.email
                };

                return View(model);
            }
            else
            {
                return NotFound(); 
            }
        }

        public IActionResult Showtruf()
        {
            var pro = _context.Grounds.ToList();
            return new JsonResult(pro);
        }

        public IActionResult MoreDetail(int id)
        {
            var send = _context.Grounds.Where(x => x.Id == id).ToList();
            return View(send);
        }

        [HttpPost]
        //public IActionResult CheckAvailability(string groundName, string date, string time)
        //{

        //    bool isAvailable = !_context.Bookingkmodel.Any(b =>
        //        b.GroundName == groundName &&
        //        b.Date == date &&
        //        b.Time == time
        //    );

        //    return Json(new { isAvailable });
        //}
        public IActionResult CheckAvailability(string groundName, string date, string time)
        {
            bool isAvailable = false;

            if (time == "Full Day")
            {

                isAvailable = !_context.Bookingkmodel.Any(b =>
                    b.GroundName == groundName &&
                    b.Date == date
                );
            }
            else
            {

                isAvailable = !_context.Bookingkmodel.Any(b =>
                    b.GroundName == groundName &&
                    b.Date == date &&
                    (b.Time == "Full Day" || b.Time == time)
                );
            }

            return Json(new { isAvailable });
        }

        public IActionResult ConfirmBooking(BookingModel booking)
        {
            _context.Bookingkmodel.Add(booking);
            _context.SaveChanges();
            return Json(new { BookingSuccess = true });
        }
        public IActionResult BookingDetail()
        {
            return View();
        }

    }
}
