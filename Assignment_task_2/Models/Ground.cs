using System.ComponentModel.DataAnnotations;

namespace Assignment_task_2.Models
{
    public class Ground
    {
        [Key]
        public int Id { get; set; }

        public string GroundName { get; set; }

        public string Description { get; set; }

        public string Location { get; set; }
        public string Price { get; set; }
        public byte[] Image { get; set; }
    }
}
