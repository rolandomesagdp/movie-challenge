using CinemaManager.Movies;
using CinemaManager.Tickets;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CinemaManager.Showtimes
{
    public class ShowtimeEntity
    {
        public int Id { get; set; }
        public MovieEntity Movie { get; set; }
        public DateTime SessionDate { get; set; }
        public int AuditoriumId { get; set; }
        public List<TicketEntity> Tickets { get; set; }

        public ShowtimeEntity()
        {
            Tickets = new List<TicketEntity>();
        }

        public bool IsValid()
        {
            return Movie != null && SessionDate > DateTime.Today;
        }

        public void ClearExpiredReservations()
        {
            var expiredReservations = Tickets.FindAll(x => x.CreatedTime <= DateTime.UtcNow.AddMinutes(-10));
            foreach (var reservation in expiredReservations)
            {
                Tickets.Remove(reservation);
            }
        }

        public TicketReservationResponse Reserve(TicketEntity ticketToReserve)
        {
            if (ticketToReserve != null && ticketToReserve.IsValid()) 
            {
                Tickets.Add(ticketToReserve);
                return new TicketReservationResponse
                {
                    Id = ticketToReserve.Id,
                    NumberOfSeats = ticketToReserve.Seats.Count(),
                    AuditoriumId = AuditoriumId,
                    MovieTitle = Movie.Title,
                    SessionDate = SessionDate
                };
            }
            throw new ArgumentException("The provided ticket reservation is not valid");
        }
    }
}
