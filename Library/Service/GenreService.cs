using Library.DBContext;
using Library.Interfaces;
using Library.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using static Library.Service.BookService;

namespace Library.Service
{
    public class GenreService : IGenreService
    {
        private readonly APIDB _context;
        public GenreService(APIDB context)
        {
            _context = context;
        }

        public class PagePag
        {
            public int Page { get; set; } = 1;
            public int PageSize { get; set; } = 10;
        }

        PagePag pagepag = new PagePag();

        public async Task<ActionResult<IEnumerable<Genre>>> GetGenres([FromQuery] PagePag pag)
        {
            var p = pag.Page;
            var ps = pag.PageSize;
            var TotalCount = (await _context.Genre.ToListAsync()).Count();
            var TotalPages = (int)Math.Ceiling((decimal)TotalCount / ps);
            var GenresPerPage = await _context.Genre
                .Skip((p - 1) * ps)
                .Take(ps)
                .ToListAsync();
            return (GenresPerPage);
        }


        public async Task<ActionResult<Genre>> GetGenre(int id)
        {
            var genre = await _context.Genre.FindAsync(id);
            if (genre == null)
            {
                return null; //(new { Message = "Жанр не найден в базе данных." });
            }
            return (genre);
        }

        public async Task<ActionResult<Genre>> PostGenre([FromBody] Genre genre)
        {
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(genre, new ValidationContext(genre), validationResults);
            if (!isValid)
            {
                return null;// (ModelState);
            }

            _context.Genre.Add(genre);

            await _context.SaveChangesAsync();

            return null; // (nameof(GetGenre), new { id = genre.ID_Genre }, genre);
        }

        public async Task<IActionResult> PutGenre(int id, [FromBody] Genre genre)
        {
            if (id != genre.ID_Genre)
            {
                return null; // BadRequest(new { Message = "ID жанра не совпадают." });
            }

            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(genre, new ValidationContext(genre), validationResults);

            if (!isValid)
            {
                return null;// (ModelState);
            }

            _context.Entry(genre).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GenreExists(id))
                {
                    return null; // NotFound(new { Message = "Жанр не найден в базе данных." });
                }
                return null; //(StatusCodes.Status500InternalServerError)
            }

            return null; // NoContentResult(); // Код 204
        }

        public async Task<IActionResult> DeleteGenre(int id)
        {
            var genre = await _context.Genre.FindAsync(id);
            if (genre == null)
            {
                return null; // NotFound(new { Message = "Жанр не найден в базе данных." });
            }

            _context.Genre.Remove(genre);
            await _context.SaveChangesAsync();

            return new NoContentResult(); // NoContent(); // Код 204
        }

        private bool GenreExists(int id)
        {
            return _context.Genre.Any(e => e.ID_Genre == id);
        }
    }
}
