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
    public class Rents_Controll : Controller
    {

        private readonly APIDB _context;

        public Rents_Controll(APIDB context)
        {
            _context = context;
        }

        // Получение данных аренды по читателю
        [HttpGet("history/reader/{id}")]
        public async Task<ActionResult<IEnumerable<Rents>>> GetRentalHistoryByReader(int id)
        {
            var rentals = await _context.Rents.Where(r => r.ID_Reader == id).ToListAsync();
            if (rentals == null || rentals.Count == 0)
            {
                return NotFound(new { Message = "Аренд для этого читателя не найдена в базе данных" });
            }
            return Ok(rentals);
        }

        // Получение данных аренды по книге
        [HttpGet("history/book/{id}")]
        public async Task<ActionResult<IEnumerable<Rents>>> GetRentalHistoryByBook(int id)
        {
            var rentals = await _context.Rents.Where(r => r.ID_Book == id).ToListAsync();
            if (rentals == null || rentals.Count == 0)
            {
                return NotFound(new { Message = "Аренд для этой книги не найдено в базе данных" });
            }
            return Ok(rentals);
        }

        // Получение текущих аренд
        [HttpGet("current")]
        public async Task<ActionResult<IEnumerable<Rents>>> GetCurrentRentals()
        {
            var currentRentals = await _context.Rents.Where(r => r.Return_Date == null).ToListAsync();
            if (currentRentals == null || currentRentals.Count == 0)
            {
                return NotFound(new { Message = "Текущих аренд не найдено." });
            }
            return Ok(currentRentals);
        }

        // Получение аренды по его ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Rents>> GetRental(int id)
        {
            var rental = await _context.Rents.FindAsync(id);
            if (rental == null)
            {
                return NotFound(new { Message = "Аренда отсутствует в базе данных." });
            }
            return Ok(rental);
        }

        // Добавление данных аренды в базу данных
        [HttpPost]
        public async Task<ActionResult<Rents>> PostRental([FromBody] Rents rental)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Rents.Add(rental);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetRental), new { id = rental.ID_Rental }, rental);
        }

        // Изменение данных аренды по его ID
        [HttpPut("return/{id}")]
        public async Task<IActionResult> ReturnRental(int id)
        {
            var rental = await _context.Rents.FindAsync(id);
            if (rental == null)
            {
                return NotFound(new { Message = "Аренда отсутствует в базе данных" });
            }

            _context.Rents.Remove(rental);
            await _context.SaveChangesAsync();

            return NoContent(); // Код 204

        }
    }

}
