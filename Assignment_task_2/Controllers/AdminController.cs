using Assignment_task_2.Models;
using Microsoft.AspNetCore.Mvc;

namespace Assignment_task_2.Controllers
{
    public class AdminController : Controller
    {
        private readonly DataBaseConnect _context;
        public AdminController(DataBaseConnect context)
        {
            _context = context;
        }
        public IActionResult Adminlogin ()
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
    }
}
