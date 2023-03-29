using CinemaManager.Tickets;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ApiApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketsController : ControllerBase
    {
        private ITicketsUseCase _ticketsUseCase;

        public TicketsController(ITicketsUseCase ticketsUseCase)
        {
            _ticketsUseCase = ticketsUseCase;
        }

        // GET: api/<ShowtimeController>
        [HttpPost("reserve")]
        public async Task<IActionResult> Reserve([FromBody] TicketReservationRequest reservationRequest, CancellationToken cancel)
        {
            try
            {
                var seatsReservation = await _ticketsUseCase.ReserveAsync(reservationRequest, cancel);
                return Ok(seatsReservation);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("confirm")]
        public async Task<IActionResult> ConfirmTicket([FromBody]Guid ticketId, CancellationToken cancel)
        {
            try
            {
                var cancelationTokenSource = new CancellationTokenSource();
                var confirmedTicket = await _ticketsUseCase.ConfirmAsync(ticketId, cancel);
                return Ok(confirmedTicket);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
