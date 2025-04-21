namespace MyMvcApp.Models
{
    public class ScheduleRating
    {
        public int Id { get; set; }
        public string UserId { get; set; } 
        public int Rating { get; set; } 
        public DateTime CreatedAt { get; set; } 
        public int ScheduleId { get; set; } 

        public Schedule Schedule { get; set; }
        public ApplicationUser User { get; set; } 
    }

}