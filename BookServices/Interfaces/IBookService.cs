using Library.Model;
using Microsoft.AspNetCore.Mvc;
using static Library.Service.BookService;

namespace Library.Interfaces
{
    public interface IBookService
    {
        Task<ActionResult<IEnumerable<Book>>> GetBookFilter([FromQuery] FilterBook filter);
        Task<ActionResult<IEnumerable<Book>>> GetBooks([FromQuery] PagePag pag);

        Task<ActionResult<Book>> GetBook(int id);

        Task<ActionResult<int>> GetAvailableCopies(int id);

        Task<IActionResult> DeleteBook(int id);

        Task<ActionResult<Book>> PostBook([FromBody] Book book);

        Task<IActionResult> PutBook(int id, [FromBody] Book book);
        //Task<ActionResult<Book>> PostBook([FromBody] Book book);
    }
}
