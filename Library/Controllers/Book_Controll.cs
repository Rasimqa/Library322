using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Library.DBContext;
using Library.Model;
using Library.Interfaces;
using static Library.Service.BookService;

namespace Library.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class Book_Controll : IBookService
    {

        private readonly IBookService _bookService;
        public Book_Controll(IBookService bookService)
        {
            _bookService = bookService;
        }
        // Получение списка всех книг находящиеся в Базе данных
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooks([FromQuery] PagePag pag)
        {
            return await _bookService.GetBooks(pag);
        }

        // Получение книги по её ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBook(int id)
        {
           return await _bookService.GetBook(id);
        }

        [HttpGet("{id}/availability")]
        public async Task<ActionResult<int>> GetAvailableCopies(int id)
        {
            return await _bookService.GetAvailableCopies(id);
        }

        // Создание/Добавление книги в базу данных
        [HttpPost]
        public async Task<ActionResult<Book>> PostBook([FromBody] Book book)
        {
            return await _bookService.PostBook(book);
        }

        // Изменение/Обновление данных книги по её ID
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBook(int id, [FromBody] Book book)
        {
            return await _bookService.PutBook(id, book);
        }

        // Удаление книги из базы данных
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            return await  _bookService.DeleteBook(id);
        }

        //private bool BookExists(int id)
        //{
        //    return _bookService.Book.Any(e => e.ID_Book == id);
        //}

        //}
    }
}

