using System.Collections.Generic;
using System.Threading.Tasks;
using SportTrack_Sigdef.Controladores.SaaS.Dtos;

namespace SportTrack_Sigdef.Controladores.SaaS
{
    public interface ISaaSService
    {
        Task<IEnumerable<PlanSaaSDto>> GetPlanesAsync();
        Task<PlanSaaSDto> GetPlanByIdAsync(int id);
        Task AsignarPlanAClubAsync(int clubId, int planId);
        Task<IEnumerable<ClubSaaSStatusDto>> GetClubesStatusAsync();
        Task ToggleClubActivoAsync(int clubId);
        Task<int> CreateFederacionWithAdminAsync(SaaSCreateFederacionDto dto);
        Task<GlobalMetricsDto> GetGlobalMetricsAsync();
    }
}
