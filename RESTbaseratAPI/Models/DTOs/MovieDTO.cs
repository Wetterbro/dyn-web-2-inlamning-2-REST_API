using System.ComponentModel.DataAnnotations;

namespace RESTbaseratAPI.DTO;

public class MovieDTO
{
    [Required]
    [MinLength(1),MaxLength(100)]
    public string Title { get; set; }
    
    [Required]
    public DateTime ReleaseYear { get; set; }
}