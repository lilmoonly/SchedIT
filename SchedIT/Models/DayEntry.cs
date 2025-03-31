using System.ComponentModel.DataAnnotations;

namespace MyMvcApp.Models
{
    public class DayEntry
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Value { get; set; }
    }
}