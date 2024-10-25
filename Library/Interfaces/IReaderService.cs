using Library.Model;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using static Library.Service.ReaderService;

namespace Library.Interfaces
{
    public interface IReaderService
    {
        Task<ActionResult<IEnumerable<Reader>>> GetReaders([FromQuery] PagePag pag);

        Task<ActionResult<IEnumerable<Reader>>> GetReaderFilter([FromQuery] FilterReader filter); 

        Task<ActionResult<Reader>> GetReader(int id);

        Task<ActionResult<Reader>> PostReader([FromBody] Reader reader);

        Task<IActionResult> PutReader(int id, [FromBody] Reader reader);

        Task<IActionResult> DeleteReader(int id);

        Task<ActionResult<IEnumerable<Rents>>> GetReaderRentals(int id);
    }
}
