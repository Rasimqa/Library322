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
    public class Reader_Controll : ControllerBase
    {
        private readonly APIDB _context;

        public Reader_Controll(APIDB context)
        {
            _context = context;
        }

        // Получение списка всех читателей
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Reader>>> GetReaders()
        {
            var readers = await _context.Reader.ToListAsync();
            if (readers == null || !readers.Any())
            {
                return NotFound(new { Message = "Читатели отсутствуют в базе данных." });
            }
            return Ok(readers);
        }

        // Получение данных читателя по его ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Reader>> GetReader(int id)
        {
            var reader = await _context.Reader.FindAsync(id);
            if (reader == null)
            {
                return NotFound(new { Message = "Читатель не найден в базе данных." });
            }
            return Ok(reader);
        }

        // Добавление читателя в базу данных
        [HttpPost]
        public async Task<ActionResult<Reader>> PostReader([FromBody] Reader reader)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "Введены некорректные данные.", Errors = ModelState });
            }

            await _context.Reader.AddAsync(reader);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetReader), new { id = reader.ID_Reader }, reader);
        }

        // Изменения данных читателя в базе данных
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReader(int id, [FromBody] Reader reader)
        {
            if (id != reader.ID_Reader)
            {
                return BadRequest(new { Message = "ID читателя не совпадают." });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "Введены некорректные данные.", Errors = ModelState });
            }

            _context.Entry(reader).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReaderExists(id))
                {
                    return NotFound(new { Message = "Читатель не найден в базе данных." });
                }
                return StatusCode(StatusCodes.Status500InternalServerError, "Ошибка при обновлении/изменении данных читателя.");
            }

            return NoContent(); // Возврат 204 No Content
        }

        // Удаление читателя из базы данных
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReader(int id)
        {
            var reader = await _context.Reader.FindAsync(id);
            if (reader == null)
            {
                return NotFound(new { Message = "Читатель не найден в базе данных." });
            }

            _context.Reader.Remove(reader);
            await _context.SaveChangesAsync();

            return NoContent(); // Возврат 204 No Content
        }

        // Получение аренды по читателю
        [HttpGet("{id}/rentals")]
        public async Task<ActionResult<IEnumerable<Rents>>> GetReaderRentals(int id)
        {
            var rentals = await _context.Rents.Where(r => r.ID_Reader == id).ToListAsync();
            if (rentals == null || !rentals.Any())
            {
                return NotFound(new { Message = "Отсутствуют аренды для этого читателя." });
            }
            return Ok(rentals);
        }

        private bool ReaderExists(int id)
        {
            return _context.Reader.Any(e => e.ID_Reader == id);
        }
    }
}
