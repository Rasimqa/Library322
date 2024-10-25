using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Library.DBContext;
using Library.Model;
using Library.Interfaces;
using static Library.Service.ReaderService;


namespace Library.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class Reader_Controll : IReaderService
    {

        private readonly IReaderService _readerService;
        public Reader_Controll(IReaderService readerService)
        {
            _readerService = readerService;
        }

        // Получение списка всех читателей
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Reader>>> GetReaders([FromQuery] PagePag pag)
        {
            return await _readerService.GetReaders(pag);
        }

        [HttpGet("ReaderFilter")]
        public async Task<ActionResult<IEnumerable<Reader>>> GetReaderFilter([FromQuery] FilterReader filter)
        {
            return await _readerService.GetReaderFilter(filter);
        }

        // Получение данных читателя по его ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Reader>> GetReader(int id)
        {
           return await _readerService.GetReader(id);
        }

        // Добавление читателя в базу данных
        [HttpPost]
        public async Task<ActionResult<Reader>> PostReader([FromBody] Reader reader)
        {
            return await _readerService.PostReader(reader);
        }

        // Изменения данных читателя в базе данных
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReader(int id, [FromBody] Reader reader)
        {
           return await _readerService.PutReader(id, reader);
        }

        // Удаление читателя из базы данных
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReader(int id)
        {
            return await _readerService.DeleteReader(id);
        }

        // Получение аренды по читателю
        [HttpGet("{id}/rentals")]
        public async Task<ActionResult<IEnumerable<Rents>>> GetReaderRentals(int id)
        {
            return await _readerService.GetReaderRentals(id);
        }

    }
}
