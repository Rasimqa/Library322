using Library.DBContext;
using Library.Interfaces;
using Library.Migrations;
using Library.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Reflection.PortableExecutable;

namespace Library.Service
{
    public class RentService : IRentService
    {
        private readonly APIDB _context;
        public RentService(APIDB context)
        {
            _context = context;
        }

        public class PagePag
        {
            public int Page { get; set; } = 1;
            public int PageSize { get; set; } = 10;
        }

        PagePag pagepag = new PagePag();

        public async Task<ActionResult<IEnumerable<Rents>>> GetRentalHistoryByReader(int id)
        {
            var rentals = await _context.Rents.Where(r => r.ID_Reader == id).ToListAsync();
            if (rentals == null || rentals.Count == 0)
            {
                return null; // NotFound(new { Message = "Аренд для этого читателя не найдена в базе данных" });
            }
            return (rentals);
        }

        public async Task<ActionResult<IEnumerable<Rents>>> GetRentalHistoryByBook(int id)
        {
            var rentals = await _context.Rents.Where(r => r.ID_Book == id).ToListAsync();
            if (rentals == null || rentals.Count == 0)
            {
                return null; // NotFound(new { Message = "Аренд для этой книги не найдено в базе данных" });
            }
            return (rentals);
        }

        public async Task<ActionResult<IEnumerable<Rents>>> GetCurrentRentals([FromQuery] PagePag pag)
        {
            //var currentRentals = await _context.Rents.Where(r => r.Return_Date == null).ToListAsync();
            //if (currentRentals == null || currentRentals.Count == 0)
            //{
            //    return NotFound(new { Message = "Текущих аренд не найдено." });
            //}
            //return Ok(currentRentals);

            var p = pag.Page;
            var ps = pag.PageSize;
            var TotalCount = (await _context.Rents.ToListAsync()).Count();
            var TotalPages = (int)Math.Ceiling((decimal)TotalCount / ps);
            var RentsPerPage = await _context.Rents
                .Skip((p - 1) * ps)
                .Take(ps)
                .ToListAsync();
            return (RentsPerPage);
        }

        public async Task<ActionResult<Rents>> GetRental(int id)
        {
            var rental = await _context.Rents.FindAsync(id);
            if (rental == null)
            {
                return null; // NotFound(new { Message = "Аренда отсутствует в базе данных." });
            }
            return (rental);
        }

        public async Task<ActionResult<Rents>> PostRental([FromBody] Rents rental)
        {
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(rental, new ValidationContext(rental), validationResults);
            if (!isValid)
            {
                return null; // BadRequest(ModelState);
            }

            _context.Rents.Add(rental);
            await _context.SaveChangesAsync();

            return null; // CreatedAtAction(nameof(GetRental), new { id = rental.ID_Rental }, rental);
        }

        public async Task<IActionResult> ReturnRental(int id)
        {
            var rental = await _context.Rents.FindAsync(id);
            if (rental == null)
            {
                return null; // NotFound(new { Message = "Аренда отсутствует в базе данных" });
            }

            _context.Rents.Remove(rental);
            await _context.SaveChangesAsync();

            return new NoContentResult();  // Код 204

        }
    }
}
