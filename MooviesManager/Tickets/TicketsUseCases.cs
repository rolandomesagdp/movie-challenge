using CinemaManager.Auditoriums;
using CinemaManager.Showtimes;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CinemaManager.Tickets
{
    public class TicketsUseCases : ITicketsUseCase
    {
        private readonly IShowtimesRepository _showtimeRepository;
        private readonly IAuditoriumsRepository _auditoriumRepository;
        private readonly ITicketsRepository _ticketsRepository;

        public TicketsUseCases(IShowtimesRepository showtimeRepository, IAuditoriumsRepository auditoriumRepository, ITicketsRepository ticketsRepository)
        {
            _showtimeRepository = showtimeRepository;
            _auditoriumRepository = auditoriumRepository;
            _ticketsRepository = ticketsRepository;
        }

        public async Task<TicketEntity> ConfirmAsync(Guid reservationId, CancellationToken cancel)
        {
            var ticketToConfirm = await _ticketsRepository.GetAsync(reservationId, cancel);
            if (ticketToConfirm != null)
            {
                ticketToConfirm.Confirm();
                await _ticketsRepository.SaveChanges();
                return ticketToConfirm;
            }
            throw new ArgumentException($"The current reservation does not exists. Either the id is not correct or the reservation was deleted after expiration. Provided reservation id: {reservationId}");
        }

        public async Task<TicketReservationResponse> ReserveAsync(TicketReservationRequest ticketRequest, CancellationToken cancel)
        {
            var auditorium = await _auditoriumRepository.GetAsync(ticketRequest.AuditoriumId, cancel);
            var showtime = await _showtimeRepository.GetWithTicketsAndMoviesByIdAsync(ticketRequest.ShowtimeId, cancel);
            showtime.ClearExpiredReservations();
            var ticket = BuildTicket(ticketRequest, auditorium, showtime);
            var reservation = showtime.Reserve(ticket);
            await _showtimeRepository.SaveChanges();
            return reservation;
        }

        private TicketEntity BuildTicket(TicketReservationRequest ticketRequest, AuditoriumEntity auditorium, ShowtimeEntity showtime)
        {
            var ticketFactory = new TicketFactory(ticketRequest, auditorium, showtime);
            ticketFactory.Create();
            if (ticketFactory.Ticket != null) return ticketFactory.Ticket;
            else throw new ArgumentException(ticketFactory.TicketCreationError);
        }
    }
}
