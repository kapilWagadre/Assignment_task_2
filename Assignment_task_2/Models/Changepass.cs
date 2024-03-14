using System.ComponentModel.DataAnnotations;

namespace Assignment_task_2.Models
{
    public class Changepass
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string CurrentPassword {  get; set; }
        [Required]
        public string NewPassword { get; set; }

        [Required]
        [Compare("NewPassword" , ErrorMessage ="Confirm new password does not match")]
        public string ConfirmPassword { get; set; }
    }
}
