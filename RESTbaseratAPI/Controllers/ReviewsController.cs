using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RESTbaseratAPI.Data;
using RESTbaseratAPI.DTO;
using RESTbaseratAPI.Models;

namespace RESTbaseratAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ReviewsController(ApplicationDbContext context)
        {
            _context = context;
        }
        
        
        // GET: api/Reviews
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Review>>> GetReviews()
        {
            return await _context.Reviews.ToListAsync();
        }

        // GET: api/Reviews (id)
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<Review>> GetReview(int id)
        {
            var review = await _context.Reviews.FindAsync(id);

            if (review == null)
            {
                return NotFound();
            }

            return review;
        }
        
        // GET: api/Reviews/movie (id)
        [HttpGet("(user)")]
        public async Task<ActionResult<IEnumerable<Review>>> GetReviewsByUser()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userReviews = await _context.Reviews.Where(r => r.UserId == userId).ToListAsync();

            if (!userReviews.Any())
            {
                return NotFound();
            }

            return userReviews;
        }

        // POST: api/Reviews
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ReviewDTO>> PostReview(ReviewDTO reviewDTO)
        {
            // Map ReviewDTO to Review
            var review = new Review
            {
                MovieId = reviewDTO.MovieId,
                Comment = reviewDTO.Comment,
                Rating = reviewDTO.Rating,
                UserId = User.FindFirstValue(ClaimTypes.NameIdentifier)
            };

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetReview", new { id = review.Id }, reviewDTO);
        }

        // PUT: api/Reviews (id)
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReview(int id, ReviewDTO updatedReviewDTO)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var review = await _context.Reviews.FindAsync(id);

            if (review == null)
            {
                return NotFound();
            }

            if (review.UserId != userId)
            {
                return Unauthorized();
            }
            
            review.MovieId = updatedReviewDTO.MovieId;
            review.Comment = updatedReviewDTO.Comment;
            review.Rating = updatedReviewDTO.Rating;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReviewExists(id))
                {
                    return NotFound();
                }
                
                throw;
            }
            
            return NoContent();
        }


        // DELETE: api/Reviews (id)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReview(int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            if (review == null)
            {
                return NotFound();
            }
            
            if (review.UserId != userId)
            {
                return Unauthorized();
            }

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        private bool ReviewExists(int id)
        {
            return _context.Reviews.Any(e => e.Id == id);
        }
    }
}