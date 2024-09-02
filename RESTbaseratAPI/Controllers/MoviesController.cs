using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RESTbaseratAPI.Data;
using RESTbaseratAPI.DTO;
using RESTbaseratAPI.Models;

namespace RESTbaseratAPI.Controllers
{
    // MoviesController.cs
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MoviesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MoviesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        [HttpGet]
        // GET: api/Movies
        public async Task<ActionResult<IEnumerable<Movie>>> GetMovies()
        {
            var movies = await _context.Movies.Include(m => m.Reviews).ToListAsync();

            if (!movies.Any())
            {
                return NotFound();
            }

            return movies;
        }

        [AllowAnonymous]
        // GET: api/Movies (id)
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMovieById(int id)
        {
            var movie = await _context.Movies
                .Include(m => m.Reviews)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (movie == null)
                return NotFound();

            return Ok(movie);
        }

        [AllowAnonymous]
        // GET: api/Movies (id)/reviews
        [HttpGet("{id}/reviews")]
        public async Task<ActionResult<IEnumerable<Review>>> GetReviewsForMovie(int id)
        {
            var reviews = await _context.Reviews.Where(r => r.MovieId == id).ToListAsync();

            if (!reviews.Any())
            {
                return NotFound();
            }

            return Ok(reviews);
        }

        // POST: api/Movies
        [HttpPost]
        public IActionResult AddMovie([FromBody] MovieDTO movieDto)
        {
            // Map MovieDTO to Movie
            var movie = new Movie
            {
                Title = movieDto.Title,
                ReleaseYear = movieDto.ReleaseYear,
            };

            _context.Movies.Add(movie);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetMovieById), new { id = movie.Id }, movie);
        }

        // PUT: api/Movies (id)
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMovie(int id, [FromBody] MovieDTO updatedMovieDto)
        {
            var movie = await _context.Movies.FindAsync(id);

            if (movie == null)
            {
                return NotFound();
            }

            // Update properties from MovieDTO
            movie.Title = updatedMovieDto.Title;
            movie.ReleaseYear = updatedMovieDto.ReleaseYear;

            try
            {
                _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Movies (id)
        [HttpDelete("{id}")]
        public IActionResult DeleteMovie(int id)
        {
            var movie = _context.Movies.Find(id);

            if (movie == null)
                return NotFound();

            _context.Movies.Remove(movie);
            _context.SaveChanges();

            return NoContent();
        }

        private bool MovieExists(int id)
        {
            return _context.Movies.Any(e => e.Id == id);
        }
    }
}