using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Library.DBContext;
using Library.Model;


namespace Library.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class Genre_Controll : ControllerBase
    {
        private readonly APIDB _context;

        public Genre_Controll(APIDB context)
        {
            _context = context;
        }

        // Получение списка всех жанров находящиеся в Базе данных
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Genre>>> GetGenres()
        {
            var genres = await _context.Genre.ToListAsync();
            if (genres == null || genres.Count == 0)
            {
                return NotFound(new { Message = "Жанры отсутствуют в базе данных." });
            }
            return Ok(genres);
        }

        // Получение жанра по её ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Genre>> GetGenre(int id)
        {
            var genre = await _context.Genre.FindAsync(id);
            if (genre == null)
            {
                return NotFound(new { Message = "Жанр не найден в базе данных." });
            }
            return Ok(genre);
        }

        // Создание/Добавление жанра в базу данных
        [HttpPost]
        public async Task<ActionResult<Genre>> PostGenre([FromBody] Genre genre)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Genre.Add(genre);

            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetGenre), new { id = genre.ID_Genre }, genre);
        }

        // Изменение/Обновление жанров по её ID
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGenre(int id, [FromBody] Genre genre)
        {
            if (id != genre.ID_Genre)
            {
                return BadRequest(new { Message = "ID жанра не совпадают." });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
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
                    return NotFound(new { Message = "Жанр не найден в базе данных." });
                }
                return StatusCode(StatusCodes.Status500InternalServerError, "Ошибка при обновлении/изменении жанра.");
            }

            return NoContent(); // Код 204
        }

        // Удаление жанра по его ID.
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGenre(int id)
        {
            var genre = await _context.Genre.FindAsync(id);
            if (genre == null)
            {
                return NotFound(new { Message = "Жанр не найден в базе данных." });
            }

            _context.Genre.Remove(genre);
            await _context.SaveChangesAsync();

            return NoContent(); // Код 204
        }

        private bool GenreExists(int id)
        {
            return _context.Genre.Any(e => e.ID_Genre == id);
        }

    }
}
