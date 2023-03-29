using CinemaManager.Auditoriums;
using CinemaManager.Movies;
using System;
using System.Linq;

namespace CinemaManager.Showtimes
{
    internal class ShowtimeFactory
    {
        public int ShowtimeId 
        { 
            get 
            {
                return CalculateNextShowtimeId();
            } 
        }
        public AuditoriumEntity Auditorium { get; private set; }

        public ShowtimeEntity Showtime { get; private set; }

        public MovieEntity Movie { get; private set; }

        public DateTime SessionDate { get; private set; }

        public ShowtimeFactory(AuditoriumEntity auditorium, MovieEntity movie, DateTime sessionDate)
        {
            Auditorium = auditorium;
            Movie = movie;
            SessionDate = sessionDate;
        }

        public ShowtimeFactory Create()
        {
            if (!SessionDateIsValid())
            {
                throw new ArgumentOutOfRangeException($"The session date should be after tomorrow. Provided date: {SessionDate.ToShortDateString()}");  
            }
            Showtime = new ShowtimeEntity
            {
                Movie = Movie,
                AuditoriumId = Auditorium.Id,
                SessionDate = SessionDate
            };
            return this;
        }

        private int CalculateNextShowtimeId()
        {
            if (Auditorium.Showtimes != null && Auditorium.Showtimes.Count > 0)
            {
                return Auditorium.Showtimes.OrderByDescending(x => x.Id).Last().Id + 1;
            }
            return 1;
        }

        private bool SessionDateIsValid()
        { 
            return SessionDate > DateTime.UtcNow.AddDays(1);
        }
    }
}
