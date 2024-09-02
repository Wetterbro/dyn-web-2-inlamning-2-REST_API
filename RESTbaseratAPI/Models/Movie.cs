using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Newtonsoft.Json;

namespace RESTbaseratAPI.Models;

public class Movie
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [MinLength(1),MaxLength(1000)]
    public string Title { get; set; }
    
    [Required]
    public DateTime ReleaseYear { get; set; }
    
    [ValidateNever]
    public List<Review> Reviews { get; set; } = new List<Review>();
    
}