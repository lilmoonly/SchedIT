using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace MyMvcApp.Models;
public class Faculty
{
    [Key]
    public int Id { get; set; }

    private string _name;

    [Required]
    public string Name
    {
        get => _name;
        set
        {
            _name = value;
            ShortName = GenerateAbbreviation(_name);
        }
    }

    [Required]
    public string ShortName { get; private set; }

    private static string GenerateAbbreviation(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return string.Empty;

        return string.Join("", name
            .Split(new[] { ' ', '-' }, StringSplitOptions.RemoveEmptyEntries)
            .Where(word => word.Length >= 3)
            .Select(word => char.ToUpper(word[0])));
    }
}
