using Library.Model;
using Microsoft.AspNetCore.Mvc;
using static Library.Service.RentService;

namespace Library.Interfaces
{
    public interface IRentService
    {
        Task<ActionResult<IEnumerable<Rents>>> GetRentalHistoryByReader(int id);

        Task<ActionResult<IEnumerable<Rents>>> GetRentalHistoryByBook(int id);

        Task<ActionResult<IEnumerable<Rents>>> GetCurrentRentals([FromQuery] PagePag pag);

        Task<ActionResult<Rents>> GetRental(int id);

        Task<ActionResult<Rents>> PostRental([FromBody] Rents rental);

        Task<IActionResult> ReturnRental(int id);

    }
}
