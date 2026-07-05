using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SportTrack_Sigdef.Entidades.Entidades;

namespace SportTrack_Sigdef.Controladores.Bote
{
    public interface IBoteRepository
    {
        Task<IEnumerable<Entidades.Entidades.Bote>> GetAllAsync();
        Task<Entidades.Entidades.Bote?> GetByIdAsync(int id);
        Task<Entidades.Entidades.Bote> CreateAsync(Entidades.Entidades.Bote bote);
        Task<Entidades.Entidades.Bote> UpdateAsync(Entidades.Entidades.Bote bote);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
