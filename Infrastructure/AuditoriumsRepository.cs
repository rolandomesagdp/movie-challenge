using CimenaInfrastructure;
using CinemaManager.Auditoriums;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class AuditoriumsRepository : IAuditoriumsRepository
    {
        private readonly CinemaContext _context;

        public AuditoriumsRepository(CinemaContext context)
        {
            _context = context;
        }

        public async Task<AuditoriumEntity> GetAsync(int auditoriumId, CancellationToken cancel)
        {
            return await _context.Auditoriums
                .Include(x => x.Seats)
                .Include(x => x.Showtimes)
                .FirstOrDefaultAsync(x => x.Id == auditoriumId, cancel);
        }

        public async Task SaveChanges(CancellationToken cancel)
        {
            await _context.SaveChangesAsync(cancel);
        }
    }
}
