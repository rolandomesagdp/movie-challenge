using System;

namespace CinemaManager.Showtimes
{
    public class ShowtimeCreationDto
    {
        public DateTime SessionDate { get; set; }
        public int AuditoriumId { get; set; }
        public string MovieId { get; set; }
    }
}
