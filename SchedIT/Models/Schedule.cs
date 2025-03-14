using System.ComponentModel.DataAnnotations;

namespace MyMvcApp.Models
{
    public class Schedule
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public string Subject { get; set; }

        [Required]
        public string Time { get; set; }
    }
}
