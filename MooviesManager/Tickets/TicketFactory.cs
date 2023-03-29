using CinemaManager.Auditoriums;
using CinemaManager.Seats;
using CinemaManager.Showtimes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CinemaManager.Tickets
{
    internal class TicketFactory
    {
        public TicketReservationRequest TicketRequest { get; private set; }

        public AuditoriumEntity Auditorium { get; set; }

        public ShowtimeEntity Showtime { get; set; }

        public TicketEntity Ticket { get; private set; }

        public string TicketCreationError { get; private set; }

        public TicketFactory(TicketReservationRequest ticketRequest, AuditoriumEntity auditorium, ShowtimeEntity showtime)
        {
            TicketRequest = ticketRequest;
            Auditorium = auditorium;
            Showtime = showtime;
        }

        public TicketFactory Create()
        {
            var seats = BuildSeats();
            var ticket = new TicketEntity
            {
                Id = Guid.NewGuid(),
                ShowtimeId = TicketRequest.ShowtimeId,
                Showtime = Showtime,
                Seats = seats
            };
            if (ticket.IsValid()) Ticket = ticket;
            else
            {
                TicketCreationError = "A reservation cannot be created with the current information";
            } 
            return this;
        }

        private ICollection<SeatEntity> BuildSeats()
        {
            var seatsFromAuditorium = Auditorium.Seats
                .Where(x => x.Row == TicketRequest.Row)
                .Where(x => TicketRequest.Seats.Any(y => y == x.SeatNumber));

            return seatsFromAuditorium.ToList();
        }
    }
}
