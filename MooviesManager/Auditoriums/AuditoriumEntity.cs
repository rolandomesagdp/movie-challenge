using CinemaManager.Seats;
using CinemaManager.Showtimes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CinemaManager.Auditoriums
{
    public class AuditoriumEntity
    {
        public int Id { get; set; }
        public List<ShowtimeEntity> Showtimes { get; set; }
        public ICollection<SeatEntity> Seats { get; set; }

        public void AddShowtime(ShowtimeEntity showtime)
        {
            if (ShowtimeIsValidToAdd(showtime))
            {
                Showtimes.Add(showtime);                
            }
        }

        public bool ShowtimeIsValidToAdd(ShowtimeEntity newShowtime)
        {
            return newShowtime.IsValid() && !ShowtimeHasDateConflict(newShowtime);
        }

        private bool ShowtimeHasDateConflict(ShowtimeEntity newShowtime)
        {
            if (Showtimes.Any(x => x.SessionDate == newShowtime.SessionDate))
            {
                return true;
            }
            return false;
        }
    }
}
