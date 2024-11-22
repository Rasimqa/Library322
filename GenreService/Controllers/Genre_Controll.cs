using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Library.DBContext;
using Library.Model;
using Library.Interfaces;
using Library.Service;
using static Library.Service.GenreServices;

namespace Library.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class Genre_Controll : IGenreService
    {
        
        private readonly IGenreService _genreService;
        public Genre_Controll(IGenreService genreService)
        {
            _genreService = genreService;
        }

        // Получение списка всех жанров находящиеся в Базе данных
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Genre>>> GetGenres([FromQuery] PagePag pag)
        {
            return await _genreService.GetGenres(pag);
        }

        // Получение жанра по её ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Genre>> GetGenre(int id)
        {
            return await _genreService.GetGenre(id);
        }

        // Создание/Добавление жанра в базу данных
        [HttpPost]
        public async Task<ActionResult<Genre>> PostGenre([FromBody] Genre genre)
        {
            return await _genreService.PostGenre(genre);
        }

        // Изменение/Обновление жанров по её ID
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGenre(int id, [FromBody] Genre genre)
        {
            return await _genreService.PutGenre(id, genre);
        }

        // Удаление жанра по его ID.
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGenre(int id)
        {
            return await _genreService.DeleteGenre(id);
        }        

    }
}
