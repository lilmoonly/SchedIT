using System.ComponentModel.DataAnnotations;

namespace MyMvcApp.Models
{
    public class TimeEntry
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Value { get; set; } // Наприклад: "08:30 - 10:00"
    }
}