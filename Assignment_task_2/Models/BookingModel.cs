using System.ComponentModel.DataAnnotations;

namespace Assignment_task_2.Models
{
    public class BookingModel
    {
        [Key]
        public int Id { get; set; }

       
        public string CustomerName { get; set; }


        public string CustomerNumber { get; set; }


        public string CustomerEmail { get; set; }

        
        public string GroundName { get; set; }

        public string Location { get; set; }

        public String Date { get; set; }


        public string Time { get; set; }
        public string Price { get; set; }


    }
}
