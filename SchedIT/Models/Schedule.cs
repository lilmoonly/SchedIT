using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyMvcApp.Models
{
    public class Schedule
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int DayEntryId { get; set; }

        [Required]
        public int SubjectId { get; set; }

        [Required]
        public int TimeEntryId { get; set; }

        [Required]
        public int TeacherId { get; set; }

        [Required]
        public int ClassroomId { get; set; }

        [Required]
        public int GroupId { get; set; }
        
        [ForeignKey("DayEntryId")]
        public DayEntry? DayEntry { get; set; }

        [ForeignKey("SubjectId")]
        public Subject? Subject { get; set; }

        [ForeignKey("TimeEntryId")]
        public TimeEntry? TimeEntry { get; set; }

        [ForeignKey("TeacherId")]
        public Teacher? Teacher { get; set; }

        [ForeignKey("ClassroomId")]
        public Classroom? Classroom { get; set; }

        [ForeignKey("GroupId")]
        public Group? Group { get; set; }
    }
}