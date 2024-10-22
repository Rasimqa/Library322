using Azure.Core;
using Library.Controllers;
using Library.DBContext;
using Library.Interfaces;
using Library.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualBasic.CompilerServices;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;

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
                return null; //Нужно вывести ошибку
            }

            _context.Book.Remove(book);
            await _context.SaveChangesAsync();

            return null; // Баг вывода ошибки 500
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
