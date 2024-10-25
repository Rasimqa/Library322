using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Library.DBContext;
using Library.Model;
using Library.Interfaces;
using static Library.Service.RentService;

namespace Library.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Rents_Controll : IRentService
    {
        private readonly IRentService _rentService;

        public Rents_Controll(IRentService rentService)
        {
            _rentService = rentService;
        }

        // Получение данных аренды по читателю
        [HttpGet("history/reader/{id}")]
        public async Task<ActionResult<IEnumerable<Rents>>> GetRentalHistoryByReader(int id)
        {
            return await _rentService.GetRentalHistoryByReader(id);
        }

        // Получение данных аренды по книге
        [HttpGet("history/book/{id}")]
        public async Task<ActionResult<IEnumerable<Rents>>> GetRentalHistoryByBook(int id)
        {
            return await _rentService.GetRentalHistoryByBook(id);
        }

        // Получение текущих аренд
        [HttpGet("current")]
        public async Task<ActionResult<IEnumerable<Rents>>> GetCurrentRentals([FromQuery] PagePag pag)
        {
            return await _rentService.GetCurrentRentals(pag);
        }

        // Получение аренды по его ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Rents>> GetRental(int id)
        {
           return await _rentService.GetRental(id);
        }

        // Добавление данных аренды в базу данных
        [HttpPost]
        public async Task<ActionResult<Rents>> PostRental([FromBody] Rents rental)
        {
           return await _rentService.PostRental(rental);
        }

        // Изменение данных аренды по его ID
        [HttpPut("return/{id}")]
        public async Task<IActionResult> ReturnRental(int id)
        {
            return await _rentService.ReturnRental(id);
        }
    }

}
