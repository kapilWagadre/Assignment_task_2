using Assignment_task_2.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;

namespace Assignment_task_2.Controllers
{
    public class AdminController : Controller
    {
        private readonly DataBaseConnect _context;
        public AdminController(DataBaseConnect context)
        {
            _context = context;
        }
        public IActionResult Adminlogin()
        {
            return View();
        }
        public IActionResult Login(string email, string password)
        {

            var correctEmail = "kapil@gmail.com";
            var correctPassword = "1114";

            if (email == correctEmail && password == correctPassword)
            {

                return RedirectToAction("AdminHome");
            }
            else
            {

                return RedirectToAction("Adminlogin");
            }
        }

        public IActionResult AddGround()
        {
            return View();
        }
        public IActionResult ViewData(Ground model, IFormFile Image)
        {
            var ground = new Ground();

            ground.Id = model.Id;
            ground.GroundName = model.GroundName;
            ground.Description = model.Description;
            ground.Location = model.Location;
            ground.Price = model.Price;

            if (Image != null && Image.Length > 0)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    Image.CopyTo(ms);
                    ground.Image = ms.ToArray();
                }
            }

            _context.Grounds.Add(ground);
            _context.SaveChanges();
            return RedirectToAction("AddGround");
        }

        public IActionResult AdminHome()
        {
            return View();
        }

        public IActionResult Show()
        {
            var cus = _context.Grounds.ToList();
            return new JsonResult(cus);
        }
        public IActionResult UpdateData(Ground emp)
        {

            _context.Grounds.Update(emp);
            _context.SaveChanges();
            return new JsonResult(new { success = true });
        }

        public IActionResult DeleteData(int id)
        {

            var emp = _context.Grounds.Find(id);
            if (emp != null)
            {
                _context.Grounds.Remove(emp);
                _context.SaveChanges();
                return new JsonResult(new { success = true });
            }
            else
            {
                return new JsonResult(new { success = false });
            }
        }
        public IActionResult CustomerTable()
        {
            return View();
        }
        public IActionResult Singupdetail()
        {
            var cus = _context.Customers.ToList();
            return new JsonResult(cus);
        }

        public IActionResult BookingTable()
        {
            return View();
        }
        public IActionResult ShowBooking()
        {
            var book = _context.Bookingkmodel.ToList();
            return new JsonResult(book);
        }

        [HttpPost]
        public IActionResult SendConfirmationEmail(string emailAddress, string customerName, string bookingDate, string bookingTime, string groundName)
        {
            try
            {
                // Create and configure the SMTP client
                using (var client = new SmtpClient("smtp.gmail.com"))
                {
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential("kapilwagadre1111@gmail.com", "epls lyay lyfc pted");
                    client.Port = 587;
                    client.EnableSsl = true;

                    // Create the email message
                    var message = new MailMessage();
                    message.From = new MailAddress("kapilwagadre1111@gmail.com");
                    message.To.Add(emailAddress);
                    message.Subject = "Booking Confirmation";
                    message.Body = $"Dear {customerName},\n\nThank you for booking. Your booking details are as follows:\n\nDate: {bookingDate}\nTime: {bookingTime}\nGround Name: {groundName}\n\nWe look forward to seeing you!\n\nBest regards,\nThe Ground Booking Team";


                    client.Send(message);
                }

                return Ok();
            }
            catch (Exception ex)
            {
                // Handle exceptions
                return StatusCode(500, "An error occurred while sending the email.");
            }
        }

        public IActionResult Turnamentapproval()
        {
            return View();
        }

        public IActionResult ViewTurnamentBooking()
        {
            var turbook = _context.TurnamentBookings.ToList();
            return new JsonResult(turbook);
        }

        public IActionResult ApproveMethod(int id)
        {
            var record = _context.TurnamentBookings.Find(id);
            if (record != null)
            {
                // Convert string dates to DateTime objects
                var startDate = DateTime.Parse(record.StartDate);
                var endDate = DateTime.Parse(record.EndDate);

                // Load data into memory and perform comparison
                var conflictingBooking = _context.TurnamentBookings
                    .AsEnumerable()  // Load data into memory
                    .FirstOrDefault(b =>
                        b.Id != id &&
                        DateTime.Parse(b.StartDate) <= endDate &&
                        DateTime.Parse(b.EndDate) >= startDate &&
                        b.sport == record.sport &&
                        b.BookingStatus == "Confirm");

                if (conflictingBooking != null)
                {
                    cancelTurnEmail(record);
                    // There's a conflicting booking that is confirmed, return an error message
                    return Json(new { success = false, message = "A conflicting confirmed booking already exists for the selected dates and sport." });
                }

                // Update BookingStatus 
                record.BookingStatus = "Confirm";

                // Update Location 
                if (record.sport == "Cricket Turnament")
                {
                    record.Location = "CT Bhopal";
                }
                else if (record.sport == "Footboll Turnament")
                {
                    record.Location = "FCT Bhopal";
                }
                else if (record.sport == "Hockey Turnament")
                {
                    record.Location = "PS Bhopal";
                }
                else if (record.sport == "Badminton Turnament")
                {
                    record.Location = "RBG Bhopal";
                }

                approvelTurnEmail(record);
                _context.SaveChanges();
                return Json(new { success = true, message = "Approved Turnament Booking Successfully:" });
            }
            else
            {
                return Json(new { success = false, message = "Record not found" });
            }
        }

        private void cancelTurnEmail(TurnamentBooking record)
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
                    mailMessage.To.Add(record.Email);
                    mailMessage.Subject = "Booking Cancellation Massage";
                    mailMessage.Body = $"Dear {record.CustomerName},\n\nYour Turnament Ground booking -  \n\n Sport Turnament - {record.sport}  \n\n Booking start Date - {record.StartDate} ,\n\n Booking End Date - {record.EndDate}  \n\n has been  cancelled. " +
                                       "\n\n booking already exists for the selected dates and sport. \n\nThank you.";

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

        private void approvelTurnEmail(TurnamentBooking record)
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
                    mailMessage.To.Add(record.Email);
                    mailMessage.Subject = "Booking Confirmation Massage";
                    mailMessage.Body = $"Dear {record.CustomerName},\n\nYour Turnament Ground booking -  \n\n Sport Turnament - {record.sport}  \n\n Booking start Date - {record.StartDate} ,\n\n Booking End Date - {record.EndDate}  \n\n has been Approved.\n\n Ground Location - {record.Location} " +
                                       "\n\n Come and play your Turnament \n\nThank you.";

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

        public IActionResult RemoveApproval(int id)
        {
            var emp = _context.TurnamentBookings.Find(id);
            if (emp != null)
            {
                _context.TurnamentBookings.Remove(emp);
                _context.SaveChanges();
                return Json(new { success = true, message = "Booking Details Remove Successfully:" });
            }
            else
            {
                return Json(new { success = true, message = "Booking not faund"});
            }
        }
    }
}
