using System;
using System.Threading;
using System.Threading.Tasks;

namespace CinemaManager.Tickets
{
    public interface ITicketsUseCase
    {
        Task<TicketReservationResponse> ReserveAsync(TicketReservationRequest ticketRequest, CancellationToken cancel);

        Task<TicketEntity> ConfirmAsync(Guid reservationId, CancellationToken cancel);
    }
}
