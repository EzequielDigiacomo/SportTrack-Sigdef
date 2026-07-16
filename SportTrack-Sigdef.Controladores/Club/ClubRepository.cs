using Microsoft.EntityFrameworkCore;
using SportTrack_Sigdef.AccesoDatos;
using SportTrack_Sigdef.Entidades.Entidades;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SportTrack_Sigdef.Controladores.Club
{
    public class ClubRepository : IClubRepository
    {
        private readonly SportTrackDbContext _context;

        public ClubRepository(SportTrackDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Entidades.Entidades.Club>> GetAllAsync()
        {
            return await _context.Clubes.Include(c => c.Participantes).Include(c => c.Federacion).Include(c => c.PlanSaaS).AsNoTracking().ToListAsync();
        }

        public async Task<Entidades.Entidades.Club?> GetByIdAsync(int id)
        {
            return await _context.Clubes
                .Include(c => c.Participantes)
                .Include(c => c.Federacion!)
                    .ThenInclude(f => f.PlanSaaS)
                .Include(c => c.PlanSaaS)
                .FirstOrDefaultAsync(c => c.IdClub == id);
        }

        public async Task<Entidades.Entidades.Club> CreateAsync(Entidades.Entidades.Club club)
        {
            _context.Clubes.Add(club);
            await _context.SaveChangesAsync();
            return club;
        }

        public async Task<Entidades.Entidades.Club> UpdateAsync(Entidades.Entidades.Club club)
        {
            _context.Entry(club).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return club;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var club = await _context.Clubes.FindAsync(id);
            if (club == null) return false;
            
            _context.Clubes.Remove(club);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Clubes.AnyAsync(e => e.IdClub == id);
        }
    }
}
