using CinemaManager.Seats;
using CinemaManager.Showtimes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CinemaManager.Tickets
{
    public class TicketEntity
    {
        public TicketEntity()
        {
            CreatedTime = DateTime.Now;
            Paid = false;
        }

        public Guid Id { get; set; }
        public int ShowtimeId { get; set; }
        public ICollection<SeatEntity> Seats { get; set; }
        public DateTime CreatedTime { get; set; }
        public bool Paid { get; set; }
        public ShowtimeEntity Showtime { get; set; }

        public bool IsValid()
        {
            return Seats != null && Seats.Count > 0 && SeatsAreValid();
        }

        public bool SeatsAreValid()
        {
            return SeatsAreInTheSameRow() && SeatsAreContiguos() && SeatsAreAvailable();
        }

        public void Confirm()
        {
            if (CanBeConfirmed())
            {
                Paid = true;
            }
            else throw new Exception($"This ticket expired at {CreatedTime.AddMinutes(10).ToLongTimeString()}. You have 10 minutes to pay for reserved tickets.");
        }

        public bool CanBeConfirmed()
        { 
            return CreatedTime >= DateTime.UtcNow.AddMinutes(-2);
        }

        private bool SeatsAreAvailable()
        {
            if (Showtime.Tickets.Count == 0) return true;

            var reservedSeats = new List<SeatEntity>();
            foreach (var ticket in Showtime.Tickets)
            {
                reservedSeats.AddRange(ticket.Seats);
            }

            var seatsConflict = reservedSeats.Any(x => Seats.Any(b => b.Row == x.Row && b.SeatNumber == x.SeatNumber));
            return !seatsConflict;
        }

        public bool SeatsAreContiguos()
        {
            if (Seats.Count == 0) return false;

            var organizedSeats = Seats.Select(x => Convert.ToInt32(x.SeatNumber)).OrderBy(x => x).ToList();
            var firstSeat = Seats.OrderBy(x => x.SeatNumber).FirstOrDefault();
            var lastSeat = Seats.OrderBy(x => x.SeatNumber).LastOrDefault();


            var sequentialSeatNumbers = Enumerable.Range(firstSeat.SeatNumber, organizedSeats.Count()).ToList();
            var seatsAreSequential = sequentialSeatNumbers.SequenceEqual(organizedSeats);
            return seatsAreSequential;
        }

        public bool SeatsAreInTheSameRow()
        {
            var firstSeat = Seats.FirstOrDefault();
            if (firstSeat == null) return false;
            return !Seats.Any(x => x.Row != firstSeat.Row);
        }
    }
}
