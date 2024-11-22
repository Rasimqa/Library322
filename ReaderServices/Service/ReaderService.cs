using Library.DBContext;
using Library.Interfaces;
using Library.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Library.Service
{
    public class ReaderService : IReaderService
    {
        private readonly APIDB _context;
        public ReaderService(APIDB context)
        {
            _context = context;
        }

        public class FilterReader
        {
            public DateOnly? DateOfBirth { get; set; }

        }

        public class PagePag
        {
            public int Page { get; set; } = 1;
            public int PageSize { get; set; } = 10;
        }

        PagePag pagepag = new PagePag();

        public async Task<ActionResult<IEnumerable<Reader>>> GetReaderFilter([FromQuery] FilterReader filter)
        {
            var select = _context.Reader.AsQueryable();

            if (filter.DateOfBirth.HasValue)
                select = select.Where(b => b.DateOfBirth == filter.DateOfBirth.Value);

            var totalItems = select.Count();

            var exec = select;
            return new OkObjectResult(exec);
        }

        public async Task<ActionResult<IEnumerable<Reader>>> GetReaders([FromQuery] PagePag pag)
        {
            var p = pag.Page;
            var ps = pag.PageSize;
            var TotalCount = (await _context.Reader.ToListAsync()).Count();
            var TotalPages = (int)Math.Ceiling((decimal)TotalCount / ps);
            var ReadersPerPage = await _context.Reader
                .Skip((p - 1) * ps)
                .Take(ps)
                .ToListAsync();
            return (ReadersPerPage);
        }

        public async Task<ActionResult<Reader>> GetReader(int id)
        {
            var reader = await _context.Reader.FindAsync(id);
            if (reader == null)
            {
                return null; // NotFound(new { Message = "Читатель не найден в базе данных." });
            }
            return (reader);
        }

        public async Task<ActionResult<Reader>> PostReader([FromBody] Reader reader)
        {
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(reader, new ValidationContext(reader), validationResults);
            if (!isValid)
            {
                return null; // BadRequest(new { Message = "Введены некорректные данные.", Errors = ModelState });
            }

            await _context.Reader.AddAsync(reader);
            await _context.SaveChangesAsync();

            return null;// CreatedAtAction(nameof(GetReader), new { id = reader.ID_Reader }, reader);
        }

        public async Task<IActionResult> PutReader(int id, [FromBody] Reader reader)
        {
            if (id != reader.ID_Reader)
            {
                return null; // BadRequest(new { Message = "ID читателя не совпадают." });
            }
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(reader, new ValidationContext(reader), validationResults);

            if (!isValid)
            {
                return null; // BadRequest(new { Message = "Введены некорректные данные.", Errors = ModelState });
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
                    return null; // NotFound(new { Message = "Читатель не найден в базе данных." });
                }
                return null; // StatusCode(StatusCodes.Status500InternalServerError, "Ошибка при обновлении/изменении данных читателя.");
            }

            return null; // Возврат 204 No Content
        }

        public async Task<IActionResult> DeleteReader(int id)
        {
            var reader = await _context.Reader.FindAsync(id);
            if (reader == null)
            {
                return null; // NotFound(new { Message = "Читатель не найден в базе данных." });
            }

            _context.Reader.Remove(reader);
            await _context.SaveChangesAsync();

            return new NoContentResult(); // NoContent(); // Возврат 204 No Content
        }

        //public async Task<ActionResult<IEnumerable<Rents>>> GetReaderRentals(int id)
        //{
        //    var rentals = await _context.Rents.Where(r => r.ID_Reader == id).ToListAsync();
        //    if (rentals == null || !rentals.Any())
        //    {
        //        return null; // NotFound(new { Message = "Отсутствуют аренды для этого читателя." });
        //    }
        //    return (rentals);
        //}

        private bool ReaderExists(int id)
        {
            return _context.Reader.Any(e => e.ID_Reader == id);
        }
    }
}
