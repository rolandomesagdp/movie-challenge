using CinemaManager.Showtimes;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
namespace ApiApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShowtimeController : ControllerBase
    {
        private readonly IShowtimeUseCase showtimeUseCase;

        public ShowtimeController(IShowtimeUseCase showtimeUseCase)
        {
            this.showtimeUseCase = showtimeUseCase;
        }

        // GET: api/<ShowtimeController>
        [HttpPost]
        public async Task<IActionResult> Get([FromBody] ShowtimeCreationDto showtimeData, CancellationToken cancel)
        {
            try
            {
                var createdShowtime = await showtimeUseCase.CreateShowtime(showtimeData, cancel);
                return Ok(createdShowtime);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message); 
            }
        }
    }
}
