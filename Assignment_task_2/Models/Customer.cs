using System.ComponentModel.DataAnnotations;

namespace Assignment_task_2.Models
{
    public class Customer
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; }
        public string contact { get; set; }
        public string address { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string cfmpassword { get; set; }

    }
}
