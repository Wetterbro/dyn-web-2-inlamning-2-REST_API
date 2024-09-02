using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace RESTbaseratAPI.Models;

public class Review
{
    [Key]
    public int Id { get; set; }
    
    [Range(1, 5)]
    public int Rating { get; set; }
    
    [Required]
    [MinLength(1),MaxLength(1000)]
    public string Comment { get; set; }
    
    [ForeignKey("MovieId")]
    [ValidateNever]
    public int MovieId { get; set; }

    [ForeignKey("UserId")]
    [ValidateNever]
    public string UserId { get; set; }
}