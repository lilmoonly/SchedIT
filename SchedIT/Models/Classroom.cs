using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyMvcApp.Models
{
    public class Classroom
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Number { get; set; } = string.Empty;

        [Required]
        public string Building { get; set; } = string.Empty;

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Capacity must be at least 1")]
        public int Capacity { get; set; }

        public string Equipment { get; set; } = string.Empty;
    }
}
