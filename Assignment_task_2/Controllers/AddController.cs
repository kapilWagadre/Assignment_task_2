using Assignment_task_2.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.AspNetCore.Authorization;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Newtonsoft.Json;
using System.Net.Mail;
using System.Net;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;

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

        public IActionResult CancelBooking(string customerName, string customerNumber, string groundName, string date)
        {
            var booking = _context.Bookingkmodel.FirstOrDefault(b => b.CustomerName == customerName &&
            b.CustomerNumber == customerNumber &&
            b.GroundName == groundName &&
            b.Date == date);
            if (booking != null)
            {
                return View(booking);
            }
            else
            {
                return NotFound("Booking details not find.");
            }

        }

        public IActionResult cancelForm(int id)
        {
            var booking = _context.Bookingkmodel.FirstOrDefault(c => c.Id == id);
            if (booking != null)
            {
                _context.Bookingkmodel.Remove(booking);
                _context.SaveChanges();
                cancelEmailSend(booking);
                return Ok("Booking cancelled successfully.");
            }
            else
            {
                return NotFound("booking details not find...");
            }
            
        }

        private void cancelEmailSend(BookingModel booking)
        {
            try
            {
                using (SmtpClient smtpClient = new SmtpClient("smtp.gmail.com"))
                {
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = new NetworkCredential("kapilwagadre1111@gmail.com", "epls lyay lyfc pted");
                    smtpClient.EnableSsl = true;
                    smtpClient.Port = 587;

                    //  email message
                    MailMessage mailMessage = new MailMessage();
                    mailMessage.From = new MailAddress("kapilwagadre1111@gmail.com");
                    mailMessage.To.Add(booking.CustomerEmail);
                    mailMessage.Subject = "Booking Cancellation Massage";
                    mailMessage.Body = $"Dear {booking.CustomerName},\n\nYour Ground booking -  \n\n Ground Name - {booking.GroundName} ,  {booking.Location} \n\n Booking Date - {booking.Date} , Time - {booking.Time} \n\n has been successfully cancelled. " +
                                       " \n\nThank you.";

                    // Send email
                    smtpClient.Send(mailMessage);
                }
            }
            catch (Exception ex)
            { 
                Console.WriteLine($"Error in SendRefundEmail method: {ex.Message}");
                throw;
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

        public IActionResult TurnamentBooking(TurnamentBooking details)
        {
            if (string.IsNullOrEmpty(details.BookingStatus))
            {
                details.BookingStatus = "Pending Approve";
            }

            if (string.IsNullOrEmpty(details.Location))
            {
                details.Location = "Not Assigned";
            }

            _context.TurnamentBookings.Add(details);
            _context.SaveChanges();

            return Json(new { success = true, message = "Tournament booking is pending. Please wait for admin approval and tournament location information via email." });
        }



        public IActionResult TurfGround(string description)
        {
            var grounds = _context.Grounds.Where(x => x.Description == description).ToList();
            return View(grounds);
        }

    }
}
