using System.Threading;
using System.Threading.Tasks;

namespace CinemaManager.Showtimes
{
    public interface IShowtimeUseCase
    {
        Task<ShowtimeEntity> CreateShowtime(ShowtimeCreationDto showtime, CancellationToken cancel);
    }
}
