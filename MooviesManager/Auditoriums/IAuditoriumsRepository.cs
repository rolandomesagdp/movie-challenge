using System.Threading;
using System.Threading.Tasks;

namespace CinemaManager.Auditoriums
{
    public interface IAuditoriumsRepository
    {
        Task<AuditoriumEntity> GetAsync(int auditoriumId, CancellationToken cancel);
        Task SaveChanges(CancellationToken cancel);
    }
}