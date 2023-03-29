using CinemaManager.Auditoriums;
using CinemaManager.Movies;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CinemaManager.Showtimes
{
    public class ShowtimeUseCases : IShowtimeUseCase
    {
        private IAuditoriumsRepository _uditoriumsRepository;
        private IMoviesReposiroty _moviesRepository;

        public ShowtimeUseCases(IAuditoriumsRepository auditoriumRepository, IMoviesReposiroty moviesRepository)
        {
            _uditoriumsRepository = auditoriumRepository;
            _moviesRepository = moviesRepository;
        }
        public async Task<ShowtimeEntity> CreateShowtime(ShowtimeCreationDto showtime, CancellationToken cancel)
        {
            var auditorium = await _uditoriumsRepository.GetAsync(showtime.AuditoriumId, cancel);
            var movie = await _moviesRepository.GetByIdAsync(showtime.MovieId, cancel);
            var newShowtime = new ShowtimeFactory(auditorium, movie, showtime.SessionDate).Create().Showtime;
            auditorium.AddShowtime(newShowtime);
            await _uditoriumsRepository.SaveChanges(cancel);
            var createdShowtime = auditorium.Showtimes.OrderByDescending(x => x.Id).FirstOrDefault();
            return createdShowtime;
        }
    }
}
