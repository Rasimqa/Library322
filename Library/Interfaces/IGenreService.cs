using Library.Model;
using Microsoft.AspNetCore.Mvc;
using static Library.Service.GenreService;

namespace Library.Interfaces
{
    public interface IGenreService
    {
        Task<ActionResult<IEnumerable<Genre>>> GetGenres([FromQuery] PagePag pag);

        Task<ActionResult<Genre>> GetGenre(int id);

        Task<ActionResult<Genre>> PostGenre([FromBody] Genre genre);

        Task<IActionResult> PutGenre(int id, [FromBody] Genre genre);

        Task<IActionResult> DeleteGenre(int id);

    }
}
