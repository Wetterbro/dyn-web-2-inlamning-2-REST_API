using Microsoft.AspNetCore.Identity;

namespace RESTbaseratAPI.Models;

public class CustomUser : IdentityUser
{
    public List<Review> Reviews { get; set; } = new List<Review>();
}