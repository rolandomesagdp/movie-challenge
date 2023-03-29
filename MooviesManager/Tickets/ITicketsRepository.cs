using CinemaManager.Seats;
using CinemaManager.Showtimes;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CinemaManager.Tickets
{
    public interface ITicketsRepository
    {
        Task<TicketEntity> ConfirmPaymentAsync(TicketEntity ticket, CancellationToken cancel);
        Task<TicketEntity> CreateAsync(ShowtimeEntity showtime, IEnumerable<SeatEntity> selectedSeats, CancellationToken cancel);
        Task<TicketEntity> GetAsync(Guid id, CancellationToken cancel);
        Task<IEnumerable<TicketEntity>> GetEnrichedAsync(int showtimeId, CancellationToken cancel);
        Task SaveChanges();
    }
}