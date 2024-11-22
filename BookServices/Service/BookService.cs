using Azure.Core;
using Library.Controllers;
using Library.DBContext;
using Library.Interfaces;
using Library.Model;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualBasic.CompilerServices;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using static Library.Service.BookService.PagePag;

namespace Library.Service
{
    public class BookService : IBookService
    {
        private readonly APIDB _context;
        public BookService(APIDB context)
        {
            _context = context;
        }      

        PagePag pagepag = new PagePag();

        public async Task<ActionResult<IEnumerable<Book>>> GetBookFilter([FromQuery] FilterBook filter)
        {
            var select = _context.Book.AsQueryable();

            if (!string.IsNullOrEmpty(filter.Title))
                select = select.Where(b => b.Name_Book.Contains(filter.Title));

            if (!string.IsNullOrEmpty(filter.Author))
                select = select.Where(b => b.Author_Name.Contains(filter.Author));

            if (!string.IsNullOrEmpty(filter.Genre))
                select = select.Where(b => Convert.ToString(b.GenreID).Contains(filter.Genre));
            DateOnly year = (filter.Year);
            if (year != null)
                select = select.Where(b => b.Year_Public == year);

            var exec = select;
            return new OkObjectResult(exec);
        }

        public async Task<ActionResult<IEnumerable<Book>>> GetBooks([FromQuery] PagePag pag)
        {
            var p = pag.Page;
            var ps = pag.PageSize;
            var TotalCount = (await _context.Book.ToListAsync()).Count();
            var TotalPages = (int)Math.Ceiling((decimal)TotalCount / ps);
            var BooksPerPage = await _context.Book
                .Skip((p - 1) * ps)
                .Take(ps)
                .ToListAsync();
            return (BooksPerPage);
        }

        public class PagePag
        {
            public int Page { get; set; } = 1;
            public int PageSize { get; set; } = 10;

            public class PagedResponse<T>
            {
                public PagedResponse(T data)
                {
                    Data = data;
                }

                public T Data { get; set; }
            }
        }

        public class FilterBook
        {
            public string Title { get; set; }
            public string Author { get; set; }
            public string Genre { get; set; }
            public DateOnly Year{ get; set; }
        }

        public async Task<ActionResult<Book>> GetBook(int id)
        {
            var book = await _context.Book.FindAsync(id);
            if (book == null)
            {
                return (null); //Нужно вывести ошибку
            }
            return (book);
        }

        public async Task<ActionResult<int>> GetAvailableCopies(int id)
        {
            var book = await _context.Book.FindAsync(id);
            if (book == null) return null; //Нужно вывести ошибку
            return book.Count_Copy;
        }

        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _context.Book.FindAsync(id);
            if (book == null)
            {
                return new NotFoundObjectResult(new { Message = "Книга отсутствует в базе данных" }); ; //Нужно вывести ошибку
            }

            _context.Book.Remove(book);
            await _context.SaveChangesAsync();

            return new NoContentResult(); ; // Баг вывода ошибки 500
        }

        public async Task<ActionResult<Book>> PostBook(Book book)
        {
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(book, new ValidationContext(book), validationResults);
            if (!isValid)
            {
                return null; // BadRequest(validationResults.Select(r => r.ErrorMessage).ToArray());
            }

            await _context.Book.AddAsync(book);
            await _context.SaveChangesAsync();

            return null;
        }

        public async Task<IActionResult> PutBook(int id, [FromBody] Book book)
        {

            bool BookExists(int id)
            {
                return _context.Book.Any(e => e.ID_Book == id);
            }

            if (id != book.ID_Book)
            {
                return null; //Нужно вывести ошибку
            }

            _context.Entry(book).State = EntityState.Modified;


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(id))
                {
                    return null; //Нужно вывести ошибку
                }
                return null;
            }

            return null; // Баг вывода ошибки 500
        }

      
    }
}
