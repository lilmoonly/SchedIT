using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyMvcApp.Models
{
    public class Teacher
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string FullName { get; set; } = string.Empty;

        [Required]
        public string Position { get; set; } = string.Empty;

        [Required]
        public string Faculty { get; set; } = string.Empty;


        public string ShortName
        {
            get
            {
                if (string.IsNullOrWhiteSpace(FullName)) return string.Empty;

                var parts = FullName.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length < 2) return FullName;

                var initials = parts[1][0] + ".";
                if (parts.Length > 2)
                    initials += " " + parts[2][0] + ".";

                return $"{parts[0]} {initials}";
            }
        }
    }
}
