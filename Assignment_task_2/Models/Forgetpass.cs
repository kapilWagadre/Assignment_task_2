using System.ComponentModel.DataAnnotations;

namespace Assignment_task_2.Models
{
    public class Forgetpass
    {
        [Required]
        public string email { get; set; }
        public bool emailsent { get; set; }
    }

}
