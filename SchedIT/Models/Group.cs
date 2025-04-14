using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyMvcApp.Models;

public class Group
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    public int FacultyId { get; set; }
    [ForeignKey("FacultyId")]
    public Faculty? Faculty { get; set; }
}