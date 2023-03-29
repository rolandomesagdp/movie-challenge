using System;

namespace CinemaManager.Tickets
{
    public class TicketReservationResponse
    {
        public Guid Id { get; set; }

        public int NumberOfSeats { get; set; }

        public int AuditoriumId { get; set; }

        public string MovieTitle { get; set; }

        public DateTime SessionDate { get; set; }
    }
}
