using System.ComponentModel.DataAnnotations;

namespace RESTbaseratAPI.DTO;

public class ReviewDTO
{
    [Required]
    public int MovieId { get; set; }
    
    [Required]
    [MinLength(1),MaxLength(500)]
    public string Comment { get; set; }
    
    [Required]
    [Range(1,5)]
    public int Rating { get; set; }
}