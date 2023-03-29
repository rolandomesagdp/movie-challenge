
using System.Threading;
using System.Threading.Tasks;

namespace CinemaManager.Movies
{
    public interface IMoviesReposiroty
    {
        Task<MovieEntity> GetByIdAsync(string id, CancellationToken cancel);
    }
}
