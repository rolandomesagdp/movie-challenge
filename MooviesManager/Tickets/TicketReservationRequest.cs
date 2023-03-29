using System.Collections.Generic;

namespace CinemaManager.Tickets
{
    public class TicketReservationRequest
    {
        public int ShowtimeId { get; set; }

        public int AuditoriumId { get; set; }

        public short Row { get; set; }

        public List<short> Seats { get; set; }
    }
}
