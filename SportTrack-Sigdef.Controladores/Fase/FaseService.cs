using AutoMapper;
using SportTrack_Sigdef.Controladores.Fase.Dtos;
using SportTrack_Sigdef.Controladores.Inscripcion;
using SportTrack_Sigdef.Controladores.Evento;
using SportTrack_Sigdef.Controladores.Fase.Progression;
using SportTrack_Sigdef.Entidades.Entidades;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportTrack_Sigdef.Controladores.Fase
{
    public interface IFaseService
    {
        Task<IEnumerable<FaseDto>> GetFasesPorEventoPruebaAsync(int eventoPruebaId);
        Task<IEnumerable<FaseDto>> GenerarFasesAutoAsync(int eventoPruebaId);
        Task<IEnumerable<FaseDto>> PromoverFasesAsync(int eventoPruebaId);
        Task<FaseDto> IniciarFaseAsync(int id, DateTime? manualStartTime = null);
        Task<FaseDto> FinalizarFaseAsync(int id);
        Task<bool> DeleteFaseAsync(int id);
        Task<FaseDto> ReiniciarFaseAsync(int id);
        Task<FaseDto> EnviarARevisionAsync(int id);
        Task<IEnumerable<FaseDto>> GetFasesPorEventoAsync(int eventoId);
        Task BatchUpdateFasesAsync(List<FaseBatchUpdateDto> dto);
        Task<IEnumerable<FaseDto>> GenerarFasesManualAsync(int eventoPruebaId, List<ManualPlacementDto> placements);
        Task UpdateResultadoStatusAsync(int resultadoId, string status);
        Task<IEnumerable<ProgressionAuditDto>> GetProgressionAuditAsync(int eventoPruebaId);
    }

    public class FaseService : IFaseService
    {
        private readonly IFaseRepository _faseRepository;
        private readonly IEtapaRepository _etapaRepository;
        private readonly IInscripcionRepository _inscripcionRepository;
        private readonly IEventoRepository _eventoRepository;

        private readonly Microsoft.AspNetCore.SignalR.IHubContext<SportTrack_Sigdef.Controladores.Hubs.TimingHub> _hubContext;
        private readonly IMapper _mapper;
        private readonly Audit.IAuditService _auditService;

        public FaseService(
            IFaseRepository faseRepository, 
            IEtapaRepository etapaRepository,
            IInscripcionRepository inscripcionRepository, 
            IEventoRepository eventoRepository,
            Microsoft.AspNetCore.SignalR.IHubContext<SportTrack_Sigdef.Controladores.Hubs.TimingHub> hubContext,
            IMapper mapper,
            Audit.IAuditService auditService)
        {
            _faseRepository = faseRepository;
            _etapaRepository = etapaRepository;
            _inscripcionRepository = inscripcionRepository;
            _eventoRepository = eventoRepository;
            _hubContext = hubContext;

            _mapper = mapper;
            _auditService = auditService;
        }

        public async Task<IEnumerable<FaseDto>> GetFasesPorEventoPruebaAsync(int eventoPruebaId)
        {
            var fases = await _faseRepository.GetByEventoPruebaIdAsync(eventoPruebaId);
            return _mapper.Map<IEnumerable<FaseDto>>(fases);
        }

        public async Task<IEnumerable<FaseDto>> GenerarFasesAutoAsync(int eventoPruebaId)
        {
            // VerificaciÃ³n de seguridad: No permitir re-sortear si ya hay resultados oficiales
            var fasesExistentes = await _faseRepository.GetByEventoPruebaIdAsync(eventoPruebaId);
            if (fasesExistentes.Any(f => f.Resultados.Any(r => r.TiempoOficial.HasValue)))
            {
                throw new InvalidOperationException("No se puede volver a sortear una regata que ya tiene resultados oficiales cargados.");
            }

            // 1. LIMPIEZA TOTAL
            await _etapaRepository.DeleteByEventoPruebaIdAsync(eventoPruebaId);
            
            var inscriptos = (await _inscripcionRepository.GetByEventoPruebaIdAsync(eventoPruebaId)).ToList();
            int inscriptosCount = inscriptos.Count;

            if (inscriptosCount == 0) return new List<FaseDto>();

            int numSeries = (int)Math.Ceiling(inscriptosCount / 9.0);
            
            var ep = await _eventoRepository.GetEventoPruebaByIdAsync(eventoPruebaId);
            
            // Asignar Plan de ProgresiÃ³n AutomÃ¡ticamente
            if (ep != null)
            {
                ep.PlanProgresionAsignado = DeterminarPlanProgresion(inscriptosCount);
                await _eventoRepository.UpdateEventoPruebaAsync(ep);
            }

            // ANCLAJE AL PROGRAMA: Usamos la hora programada del evento o de la prueba.
            DateTime nextTime;
            if (ep?.FechaHora != null)
            {
                nextTime = ep.FechaHora;
            }
            else
            {
                var baseDate = ep?.Evento?.Fecha.Date ?? DateTime.UtcNow.Date;
                var horaBase = ep?.Evento?.HoraInicioEvento ?? new TimeSpan(8, 0, 0);
                nextTime = GetUtcTime(baseDate.Add(horaBase), ep?.Evento?.TimeZoneId ?? "America/Argentina/Buenos_Aires");
            }

            if (numSeries <= 1)
            {
                var etapaFinal = new Etapa { 
                    EventoPruebaId = eventoPruebaId, 
                    Nombre = "Finales", 
                    Tipo = SportTrack_Sigdef.Entidades.Enums.TipoEtapaEnum.Final, 
                    Orden = 3
                };
                await _etapaRepository.CreateAsync(etapaFinal);

                var faseFinal = new Entidades.Entidades.Fase
                {
                    EtapaId = etapaFinal.Id,
                    NombreFase = "Final A",
                    NumeroFase = 1,
                    Estado = "Programada",
                    FechaHoraProgramada = nextTime
                };

                var priorityLanes = new int[] { 5, 4, 6, 3, 7, 2, 8, 1, 9 };
                int laneIdx = 0;
                foreach (var ins in inscriptos.OrderByDescending(x => x.EsCabezaDeSerie).ThenBy(x => x.IdInscripcion))
                {
                    faseFinal.Resultados.Add(new Entidades.Entidades.Resultado { InscripcionId = ins.IdInscripcion, Carril = priorityLanes[laneIdx++] });
                }

                await _faseRepository.CreateAsync(faseFinal);
            }
            else
            {
                var etapaElim = new Etapa { 
                    EventoPruebaId = eventoPruebaId, 
                    Nombre = "Eliminatorias", 
                    Tipo = SportTrack_Sigdef.Entidades.Enums.TipoEtapaEnum.Eliminatoria, 
                    Orden = 1 
                };
                await _etapaRepository.CreateAsync(etapaElim);

                var series = new List<List<Entidades.Entidades.Inscripcion>>();
                for (int i = 0; i < numSeries; i++) series.Add(new List<Entidades.Entidades.Inscripcion>());

                var queue = inscriptos.OrderByDescending(x => x.EsCabezaDeSerie).ThenBy(x => Guid.NewGuid()).ToList();
                int idx = 0;
                foreach (var ins in queue)
                {
                    series[idx % numSeries].Add(ins);
                    idx++;
                }

                for (int i = 0; i < numSeries; i++)
                {
                    var fase = new Entidades.Entidades.Fase
                    {
                        EtapaId = etapaElim.Id,
                        NombreFase = $"Serie {i + 1}",
                        NumeroFase = i + 1,
                        Estado = "Programada",
                        FechaHoraProgramada = nextTime
                    };

                    var priorityLanes = new int[] { 5, 4, 6, 3, 7, 2, 8, 1, 9 };
                    int laneIdx = 0;
                    foreach (var ins in series[i])
                    {
                        fase.Resultados.Add(new Entidades.Entidades.Resultado { InscripcionId = ins.IdInscripcion, Carril = priorityLanes[laneIdx++] });
                    }

                    await _faseRepository.CreateAsync(fase);
                    nextTime = nextTime.AddMinutes(10);
                }

                await PreGenerarSiguientesEtapasAsync(eventoPruebaId, inscriptosCount, numSeries, nextTime);
            }
            
            // Auditoria
            await _auditService.RegistrarAccionAsync("GENERATE_HEATS_AUTO", 
                $"Sorteo automÃ¡tico generado para la Prueba ID {eventoPruebaId}. Se crearon {numSeries} series.", null, "Competencia");

            return await GetFasesPorEventoPruebaAsync(eventoPruebaId);
        }

        private async Task PreGenerarSiguientesEtapasAsync(int eventoPruebaId, int inscriptosCount, int numSeries, DateTime nextTime)
        {
            int numSemis = GetSemifinalCount(numSeries);
            int numFinales = GetFinalCount(numSeries);

            if (numSemis > 0)
            {
                var etapaSemi = new Etapa { 
                    EventoPruebaId = eventoPruebaId, 
                    Nombre = "Semifinales", 
                    Tipo = SportTrack_Sigdef.Entidades.Enums.TipoEtapaEnum.Semifinal, 
                    Orden = 2 
                };
                await _etapaRepository.CreateAsync(etapaSemi);

                for (int i = 0; i < numSemis; i++)
                {
                    var faseSemi = new Entidades.Entidades.Fase
                    {
                        EtapaId = etapaSemi.Id,
                        NombreFase = numSemis > 1 ? $"Semifinal {i + 1}" : "Semifinal",
                        NumeroFase = i + 1,
                        Estado = "Programada",
                        FechaHoraProgramada = nextTime.AddMinutes(40 + i * 10)
                    };
                    await _faseRepository.CreateAsync(faseSemi);
                }
            }

            var etapaFinal = new Etapa { 
                EventoPruebaId = eventoPruebaId, 
                Nombre = "Finales", 
                Tipo = SportTrack_Sigdef.Entidades.Enums.TipoEtapaEnum.Final, 
                Orden = 3 
            };
            await _etapaRepository.CreateAsync(etapaFinal);

            var finalNames = new[] { "Final A", "Final B", "Final C" };
            for (int i = 0; i < numFinales; i++)
            {
                await _faseRepository.CreateAsync(new Entidades.Entidades.Fase
                {
                    EtapaId = etapaFinal.Id,
                    NombreFase = finalNames[i],
                    NumeroFase = i + 1,
                    Estado = "Programada",
                    FechaHoraProgramada = nextTime.AddMinutes(80 + i * 10)
                });
            }
        }
 
        private string DeterminarPlanProgresion(int count)
        {
            return ProgressionPlanRegistry.ResolveDefaultPlan(count);
        }

        private static int GetSemifinalCount(int numHeats) => numHeats switch
        {
            2 => 1,
            3 => 2,
            4 or 5 or 6 => 3,
            7 or 8 => 4,
            _ => Math.Max(1, (int)Math.Ceiling(numHeats / 2.0))
        };

        private static int GetFinalCount(int numHeats) => numHeats switch
        {
            <= 2 => 1,
            3 or 4 => 2,
            _ => 3
        };

        private Entidades.Entidades.Fase CrearFaseConResultados(int etapaId, string nombreFase, int numeroFase, List<Entidades.Entidades.Inscripcion> inscripcionesBase, DateTime? fechaHora = null)
        {
            var fase = new Entidades.Entidades.Fase
            {
                EtapaId = etapaId,
                NombreFase = nombreFase,
                NumeroFase = numeroFase,
                FechaHoraProgramada = fechaHora,
                Estado = "Programada"
            };

            var availableLanes = Enumerable.Range(1, 9).ToList();
            var rng = new Random();

            // 1. Asignar primero a los Cabezas de Serie (prioridad carril 5, luego 4 y 6)
            foreach (var insc in inscripcionesBase.Where(i => i.EsCabezaDeSerie).ToList())
            {
                int carrilAsignado = 0;
                if (availableLanes.Contains(5)) carrilAsignado = 5;
                else if (availableLanes.Contains(4)) carrilAsignado = 4;
                else if (availableLanes.Contains(6)) carrilAsignado = 6;
                else if (availableLanes.Any())
                {
                    int indexItem = rng.Next(availableLanes.Count);
                    carrilAsignado = availableLanes[indexItem];
                }

                if (carrilAsignado > 0)
                {
                    availableLanes.Remove(carrilAsignado);
                    fase.Resultados.Add(new Entidades.Entidades.Resultado
                    {
                        InscripcionId = insc.IdInscripcion,
                        Carril = carrilAsignado,
                        Estado = SportTrack_Sigdef.Entidades.Enums.EstadoResultadoEnum.Pendiente
                    });
                }
            }

            // 2. Asignar al resto de forma aleatoria
            foreach (var insc in inscripcionesBase.Where(i => !i.EsCabezaDeSerie).ToList())
            {
                int carrilAsignado = 0;
                if (availableLanes.Any())
                {
                    int indexItem = rng.Next(availableLanes.Count);
                    carrilAsignado = availableLanes[indexItem];
                    availableLanes.RemoveAt(indexItem);
                }

                fase.Resultados.Add(new Entidades.Entidades.Resultado
                {
                    InscripcionId = insc.IdInscripcion,
                    Carril = carrilAsignado > 0 ? carrilAsignado : null,
                    Estado = SportTrack_Sigdef.Entidades.Enums.EstadoResultadoEnum.Pendiente
                });
            }

            return fase;
        }

        public async Task<IEnumerable<FaseDto>> PromoverFasesAsync(int eventoPruebaId)
        {
            var todasLasFases = (await _faseRepository.GetByEventoPruebaIdAsync(eventoPruebaId)).ToList();
            if (!todasLasFases.Any()) return new List<FaseDto>();

            var etapas = todasLasFases.GroupBy(f => f.EtapaId)
                .Select(g => g.First().Etapa)
                .OrderBy(e => e.Orden)
                .ToList();

            var etapaCandidata = etapas.OrderByDescending(e => e.Orden)
                .Where(e => e.Tipo != SportTrack_Sigdef.Entidades.Enums.TipoEtapaEnum.Final)
                .FirstOrDefault(e =>
                {
                    var fasesDeEsaEtapa = todasLasFases.Where(f => f.EtapaId == e.Id);
                    return fasesDeEsaEtapa.Any(f => f.Resultados.Any(r => r.TiempoOficial.HasValue || r.Posicion.HasValue));
                });

            if (etapaCandidata == null)
            {
                etapaCandidata = etapas
                    .Where(e => e.Tipo != SportTrack_Sigdef.Entidades.Enums.TipoEtapaEnum.Final)
                    .OrderBy(e => e.Orden)
                    .FirstOrDefault();
            }

            if (etapaCandidata == null)
                throw new InvalidOperationException("No se encontró ninguna etapa con fases para promover.");

            var etapaActual = etapaCandidata;
            var fasesDeLaEtapa = todasLasFases.Where(f => f.EtapaId == etapaActual.Id).ToList();

            var fasesIncompletas = fasesDeLaEtapa
                .Where(f => !f.Resultados.Any() || !f.Resultados.All(r => r.TiempoOficial.HasValue || r.Posicion.HasValue))
                .Select(f => f.NombreFase)
                .ToList();

            if (fasesIncompletas.Any())
            {
                throw new InvalidOperationException(
                    $"No se puede promover la etapa '{etapaActual.Nombre}' porque faltan resultados en: {string.Join(", ", fasesIncompletas)}. " +
                    "Asegúrate de cargar y GUARDAR los tiempos de todas las series.");
            }

            var etapasAEliminar = etapas
                .Where(e => e.Orden > etapaActual.Orden)
                .Where(e => !e.Fases.Any(f => f.Resultados.Any(r => r.TiempoOficial.HasValue)))
                .ToList();

            foreach (var e in etapasAEliminar)
                await _etapaRepository.DeleteAsync(e.Id);

            todasLasFases = (await _faseRepository.GetByEventoPruebaIdAsync(eventoPruebaId)).ToList();
            etapas = todasLasFases.GroupBy(f => f.EtapaId).Select(g => g.First().Etapa).OrderBy(e => e.Orden).ToList();
            etapaActual = etapas.First(e => e.Id == etapaActual.Id);
            fasesDeLaEtapa = todasLasFases.Where(f => f.EtapaId == etapaActual.Id).OrderBy(f => f.NumeroFase).ToList();

            var ep = await _eventoRepository.GetEventoPruebaByIdAsync(eventoPruebaId);
            var inscriptos = await _inscripcionRepository.GetByEventoPruebaIdAsync(eventoPruebaId);
            var inscriptosCount = inscriptos.Count();
            var planId = ProgressionEngine.NormalizePlanId(ep?.PlanProgresionAsignado, inscriptosCount);
            var plan = ProgressionPlanRegistry.Get(planId);

            if (ep != null && string.IsNullOrWhiteSpace(ep.PlanProgresionAsignado))
            {
                ep.PlanProgresionAsignado = planId;
                await _eventoRepository.UpdateEventoPruebaAsync(ep);
            }

            var ctx = ProgressionEngine.BuildContext(fasesDeLaEtapa, f => f.Resultados);
            ProgressionResult progression;

            if (etapaActual.Tipo == SportTrack_Sigdef.Entidades.Enums.TipoEtapaEnum.Eliminatoria)
                progression = ProgressionEngine.PromoteFromEliminatoria(plan, ctx);
            else if (etapaActual.Tipo == SportTrack_Sigdef.Entidades.Enums.TipoEtapaEnum.Semifinal)
                progression = ProgressionEngine.PromoteFromSemifinal(plan, ctx);
            else
                throw new InvalidOperationException($"No se puede promover desde la etapa '{etapaActual.Nombre}'.");

            var lastFaseTime = fasesDeLaEtapa.Max(f => f.FechaHoraProgramada) ?? DateTime.UtcNow;
            var nextTime = lastFaseTime.AddMinutes(40);

            await AplicarProgresionIcfAsync(
                eventoPruebaId,
                etapaActual,
                progression,
                nextTime,
                todasLasFases,
                etapas,
                etapaActual.Tipo == SportTrack_Sigdef.Entidades.Enums.TipoEtapaEnum.Eliminatoria);

            await _auditService.RegistrarAccionAsync("PROMOTE_STAGE",
                $"Promoción ICF ({planId}) ejecutada para Prueba ID {eventoPruebaId}. Etapa: {etapaActual.Nombre}. " +
                $"Destinos: {string.Join(", ", progression.Destinos.Keys)}",
                null, "Competencia");

            return await GetFasesPorEventoPruebaAsync(eventoPruebaId);
        }

        private async Task AplicarProgresionIcfAsync(
            int eventoPruebaId,
            Etapa etapaActual,
            ProgressionResult progression,
            DateTime nextTime,
            List<Entidades.Entidades.Fase> todasLasFases,
            List<Etapa> etapas,
            bool reemplazarDestinos)
        {
            var destinosOrdenados = progression.Destinos.Keys
                .OrderBy(k => k.StartsWith("SF") ? 0 : 1)
                .ThenBy(k => k)
                .ToList();

            DateTime tempTime = nextTime;
            Etapa? etapaSemi = null;
            Etapa? etapaFinal = null;

            foreach (var destKey in destinosOrdenados)
            {
                var laneMap = progression.Destinos[destKey];
                if (!laneMap.Any()) continue;

                if (destKey.StartsWith("SF"))
                {
                    var sfNum = int.Parse(destKey[2..]);
                    etapaSemi ??= await ObtenerOCrearEtapaAsync(
                        eventoPruebaId, etapas, todasLasFases,
                        SportTrack_Sigdef.Entidades.Enums.TipoEtapaEnum.Semifinal,
                        "Semifinales", etapaActual.Orden + 1);

                    var nombre = laneMap.Count > 0 && progression.Destinos.Keys.Count(k => k.StartsWith("SF")) > 1
                        ? $"Semifinal {sfNum}" : "Semifinal";

                    await ReemplazarFaseConCarrilesAsync(
                        etapaSemi.Id, nombre, sfNum, laneMap, tempTime, todasLasFases);
                    tempTime = tempTime.AddMinutes(10);
                }
                else
                {
                    var finalName = destKey switch { "FA" => "Final A", "FB" => "Final B", "FC" => "Final C", _ => destKey };
                    var finalNum = destKey switch { "FA" => 1, "FB" => 2, "FC" => 3, _ => 1 };

                    etapaFinal ??= await ObtenerOCrearEtapaAsync(
                        eventoPruebaId, etapas, todasLasFases,
                        SportTrack_Sigdef.Entidades.Enums.TipoEtapaEnum.Final,
                        "Finales", etapaActual.Orden + (etapaSemi != null ? 2 : 1));

                    if (reemplazarDestinos)
                    {
                        await ReemplazarFaseConCarrilesAsync(
                            etapaFinal.Id, finalName, finalNum, laneMap, tempTime, todasLasFases);
                    }
                    else
                    {
                        await FusionarCarrilesEnFaseAsync(
                            etapaFinal.Id, finalName, finalNum, laneMap, tempTime, todasLasFases);
                    }
                    tempTime = tempTime.AddMinutes(10);
                }
            }
        }

        private async Task<Etapa> ObtenerOCrearEtapaAsync(
            int eventoPruebaId,
            List<Etapa> etapas,
            List<Entidades.Entidades.Fase> todasLasFases,
            SportTrack_Sigdef.Entidades.Enums.TipoEtapaEnum tipo,
            string nombre,
            int orden)
        {
            var existente = etapas.FirstOrDefault(e => e.Tipo == tipo && e.Orden >= orden);
            if (existente != null) return existente;

            var etapa = new Etapa
            {
                EventoPruebaId = eventoPruebaId,
                Nombre = nombre,
                Tipo = tipo,
                Orden = orden
            };
            await _etapaRepository.CreateAsync(etapa);
            etapas.Add(etapa);
            return etapa;
        }

        private async Task ReemplazarFaseConCarrilesAsync(
            int etapaId,
            string nombreFase,
            int numeroFase,
            Dictionary<int, Entidades.Entidades.Inscripcion> laneMap,
            DateTime? fechaHora,
            List<Entidades.Entidades.Fase> todasLasFases)
        {
            var existente = todasLasFases.FirstOrDefault(f => f.EtapaId == etapaId && f.NombreFase == nombreFase);
            if (existente != null)
            {
                await _faseRepository.DeleteAsync(existente.Id);
                todasLasFases.Remove(existente);
            }

            var fase = CrearFaseConCarriles(etapaId, nombreFase, numeroFase, laneMap, fechaHora);
            await _faseRepository.CreateAsync(fase);
            todasLasFases.Add(fase);
        }

        private async Task FusionarCarrilesEnFaseAsync(
            int etapaId,
            string nombreFase,
            int numeroFase,
            Dictionary<int, Entidades.Entidades.Inscripcion> laneMap,
            DateTime? fechaHora,
            List<Entidades.Entidades.Fase> todasLasFases)
        {
            var existente = todasLasFases.FirstOrDefault(f => f.EtapaId == etapaId && f.NombreFase == nombreFase);

            if (existente == null)
            {
                var fase = CrearFaseConCarriles(etapaId, nombreFase, numeroFase, laneMap, fechaHora);
                await _faseRepository.CreateAsync(fase);
                todasLasFases.Add(fase);
                return;
            }

            foreach (var (carril, insc) in laneMap)
            {
                var resExistente = existente.Resultados.FirstOrDefault(r => r.Carril == carril);
                if (resExistente != null)
                {
                    resExistente.InscripcionId = insc.IdInscripcion;
                    resExistente.TiempoOficial = null;
                    resExistente.Posicion = null;
                    resExistente.Estado = SportTrack_Sigdef.Entidades.Enums.EstadoResultadoEnum.Pendiente;
                }
                else
                {
                    existente.Resultados.Add(new Entidades.Entidades.Resultado
                    {
                        InscripcionId = insc.IdInscripcion,
                        Carril = carril,
                        Estado = SportTrack_Sigdef.Entidades.Enums.EstadoResultadoEnum.Pendiente
                    });
                }
            }

            await _faseRepository.UpdateAsync(existente);
        }

        private static Entidades.Entidades.Fase CrearFaseConCarriles(
            int etapaId,
            string nombreFase,
            int numeroFase,
            Dictionary<int, Entidades.Entidades.Inscripcion> laneMap,
            DateTime? fechaHora)
        {
            var fase = new Entidades.Entidades.Fase
            {
                EtapaId = etapaId,
                NombreFase = nombreFase,
                NumeroFase = numeroFase,
                FechaHoraProgramada = fechaHora,
                Estado = "Programada"
            };

            foreach (var (carril, insc) in laneMap.OrderBy(x => x.Key))
            {
                fase.Resultados.Add(new Entidades.Entidades.Resultado
                {
                    InscripcionId = insc.IdInscripcion,
                    Carril = carril,
                    Estado = SportTrack_Sigdef.Entidades.Enums.EstadoResultadoEnum.Pendiente
                });
            }

            return fase;
        }

        public async Task<IEnumerable<ProgressionAuditDto>> GetProgressionAuditAsync(int eventoPruebaId)
        {
            var todasLasFases = (await _faseRepository.GetByEventoPruebaIdAsync(eventoPruebaId)).ToList();
            var ep = await _eventoRepository.GetEventoPruebaByIdAsync(eventoPruebaId);
            var inscriptosCount = (await _inscripcionRepository.GetByEventoPruebaIdAsync(eventoPruebaId)).Count();
            var planId = ProgressionEngine.NormalizePlanId(ep?.PlanProgresionAsignado, inscriptosCount);

            var audit = new Dictionary<int, ProgressionAuditDto>();

            foreach (var fase in todasLasFases.Where(f => f.Resultados.Any(r => r.Inscripcion != null)))
            {
                var etapaTipo = fase.Etapa?.Tipo.ToString() ?? "";
                foreach (var res in fase.Resultados.Where(r => r.Inscripcion != null))
                {
                    var inscId = res.InscripcionId;
                    if (!audit.TryGetValue(inscId, out var row))
                    {
                        var p = res.Inscripcion?.Participante;
                        audit[inscId] = new ProgressionAuditDto
                        {
                            Atleta = p != null ? $"{p.Nombre} {p.Apellido}" : $"Inscripción {inscId}",
                            Plan = planId
                        };
                    }

                    var pos = res.Posicion ?? 0;
                    var label = pos > 0
                        ? $"{pos}/{fase.NombreFase} 👉 L{res.Carril}"
                        : $"{fase.NombreFase} 👉 L{res.Carril}";

                    if (etapaTipo.Contains("Eliminatoria", StringComparison.OrdinalIgnoreCase) || fase.NombreFase.StartsWith("Serie"))
                        audit[inscId].Eliminatoria = label;
                    else if (etapaTipo.Contains("Semifinal", StringComparison.OrdinalIgnoreCase))
                        audit[inscId].Semifinal = label;
                    else
                        audit[inscId].Final = label;
                }
            }

            return audit.Values.OrderBy(a => a.Atleta).ToList();
        }
        public async Task<FaseDto> IniciarFaseAsync(int id, DateTime? manualStartTime = null)
        {
            var fase = await _faseRepository.GetByIdAsync(id);
            if (fase == null) throw new KeyNotFoundException("Fase no encontrada");

            // Si el cliente nos manda su hora sincronizada (para evitar latencia de red), la usamos.
            // Si no, usamos la hora actual del servidor.
            fase.FechaHoraInicioReal = manualStartTime ?? DateTime.UtcNow;
            fase.Estado = "En Carrera";
            await _faseRepository.UpdateAsync(fase);

            // Auditoria
            await _auditService.RegistrarAccionAsync("START_RACE", 
                $"Carrera iniciada: {fase.NombreFase} (ID: {id}) a las {fase.FechaHoraInicioReal}", null, "Competencia");

            // Notificar por SignalR
            await _hubContext.Clients.Group($"race_{id}").SendAsync("RaceStarted", id, fase.FechaHoraInicioReal);
            
            // NotificaciÃ³n Global (para usuarios fuera de la regata especÃ­fica, como el Cronometrista en su Dashboard)
            await _hubContext.Clients.All.SendAsync("globalRaceStarted", id, fase.FechaHoraInicioReal);

            return _mapper.Map<FaseDto>(fase);
        }

        public async Task<FaseDto> FinalizarFaseAsync(int id)
        {
            var fase = await _faseRepository.GetByIdAsync(id);
            if (fase == null) throw new KeyNotFoundException("Fase no encontrada");

            fase.Estado = "Finalizada";
            fase.FechaHoraFinReal = DateTime.UtcNow;

            // Al finalizar oficialmente, marcamos todos los resultados como Oficiales
            if (fase.Resultados != null && fase.Resultados.Any())
            {
                // Ordenamos por tiempo para asignar posiciones automÃ¡ticamente
                var conTiempo = fase.Resultados
                                    .Where(r => r.TiempoOficial != null)
                                    .OrderBy(r => r.TiempoOficial)
                                    .ToList();

                int pos = 1;
                foreach (var res in conTiempo)
                {
                    res.Estado = SportTrack_Sigdef.Entidades.Enums.EstadoResultadoEnum.Oficial;
                    res.Posicion = pos++;
                }
            }

            await _faseRepository.UpdateAsync(fase);

            // Auditoria
            await _auditService.RegistrarAccionAsync("FINISH_RACE", 
                $"Carrera oficializada: {fase.NombreFase} (ID: {id})", null, "Competencia");

            // Notificar por SignalR (Local y Global)
            await _hubContext.Clients.Group($"race_{id}").SendAsync("RaceFinished", id);
            await _hubContext.Clients.All.SendAsync("globalRaceOfficialized", id);

            return _mapper.Map<FaseDto>(fase);
        }

        public async Task<bool> DeleteFaseAsync(int id)
        {
            await _faseRepository.DeleteAsync(id);
            return true;
        }

        public async Task<FaseDto> ReiniciarFaseAsync(int id)
        {
            var fase = await _faseRepository.GetByIdAsync(id);
            if (fase == null) throw new KeyNotFoundException("Fase no encontrada");

            // 1. Limpiar tiempos de la fase
            fase.FechaHoraInicioReal = null;
            fase.FechaHoraFinReal = null;
            fase.Estado = "Programada";

            // 2. Limpiar resultados de cada carril pero conservar la inscripciÃ³n y el carril
            if (fase.Resultados != null)
            {
                foreach (var res in fase.Resultados)
                {
                    res.TiempoOficial = null;
                    res.Posicion = null;
                    res.Estado = SportTrack_Sigdef.Entidades.Enums.EstadoResultadoEnum.Pendiente;
                }
            }

            await _faseRepository.UpdateAsync(fase);

            // Auditoria
            await _auditService.RegistrarAccionAsync("RESET_RACE", 
                $"Carrera reiniciada: {fase.NombreFase} (ID: {id}). Se limpiaron los tiempos.", null, "Competencia");

            // 3. Notificar a los clientes SignalR que la carrera se reiniciÃ³
            await _hubContext.Clients.Group($"race_{id}").SendAsync("RaceReset", id);

            return _mapper.Map<FaseDto>(fase);
        }

        public async Task<FaseDto> EnviarARevisionAsync(int id)
        {
            var fase = await _faseRepository.GetByIdAsync(id);
            if (fase == null) throw new KeyNotFoundException("Fase no encontrada");

            fase.Estado = "Pendiente de ValidaciÃ³n";
            fase.FechaHoraFinReal = DateTime.UtcNow;

            // Al enviar a revisiÃ³n, marcamos los resultados como Preliminares
            if (fase.Resultados != null)
            {
                foreach (var res in fase.Resultados)
                {
                    if (res.TiempoOficial != null)
                    {
                        res.Estado = SportTrack_Sigdef.Entidades.Enums.EstadoResultadoEnum.Preliminar;
                    }
                }
            }

            await _faseRepository.UpdateAsync(fase);
            
            // Auditoria
            await _auditService.RegistrarAccionAsync("REVIEW_RACE", 
                $"Carrera enviada a revisiÃ³n: {fase.NombreFase} (ID: {id})", null, "Competencia");

            Console.WriteLine($"[SignalR-Debug] Emitting GlobalRaceInReview for Fase {fase.Id}: {fase.NombreFase}");

            // Notificar que estÃ¡ en revisiÃ³n (Local a la carrera y Global para el Juez)
            await _hubContext.Clients.Group($"race_{id}").SendAsync("RaceInReview", id);
            await _hubContext.Clients.All.SendAsync("globalRaceInReview", new { id = fase.Id, nombre = fase.NombreFase });

            return _mapper.Map<FaseDto>(fase);
        }

        public async Task<IEnumerable<FaseDto>> GetFasesPorEventoAsync(int eventoId)
        {
            var fases = await _faseRepository.GetByEventoIdAsync(eventoId);
            return _mapper.Map<IEnumerable<FaseDto>>(fases);
        }

        public async Task BatchUpdateFasesAsync(List<FaseBatchUpdateDto> dto)
        {
            foreach (var item in dto)
            {
                var fase = await _faseRepository.GetByIdAsync(item.Id);
                if (fase != null)
                {
                    // Si el Kind es Utc, lo dejamos como estÃ¡. 
                    // Si es Unspecified (viene del string sin Z), lo tratamos como UTC para no romper la lÃ³gica de la BD,
                    // pero lo ideal es que el frontend mande el ISO con Z (como acabamos de corregir).
                    fase.FechaHoraProgramada = item.FechaHoraProgramada.Kind == DateTimeKind.Utc 
                        ? item.FechaHoraProgramada 
                        : DateTime.SpecifyKind(item.FechaHoraProgramada, DateTimeKind.Utc);
                    await _faseRepository.UpdateAsync(fase);
                }
            }
        }

        private DateTime GetUtcTime(DateTime localDateTime, string timeZoneId)
        {
            try
            {
                // Intentar bÃºsqueda estÃ¡ndar
                var tz = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
                var unspecified = DateTime.SpecifyKind(localDateTime, DateTimeKind.Unspecified);
                return TimeZoneInfo.ConvertTimeToUtc(unspecified, tz);
            }
            catch
            {
                // Fallback robusto para Argentina (UTC-3) si el servidor no reconoce el TimeZoneId
                if (timeZoneId.Contains("Argentina") || timeZoneId.Contains("Buenos_Aires"))
                {
                    return DateTime.SpecifyKind(localDateTime.AddHours(3), DateTimeKind.Utc);
                }
                // Si todo falla, asumimos que ya es UTC para evitar desvÃ­os indeterminados
                return DateTime.SpecifyKind(localDateTime, DateTimeKind.Utc);
            }
        }

        public async Task<IEnumerable<FaseDto>> GenerarFasesManualAsync(int eventoPruebaId, List<ManualPlacementDto> placements)
        {
            if (placements == null || !placements.Any())
                throw new ArgumentException("Debe proporcionar al menos una ubicaciÃ³n para generar las fases.");

            // Validar que no haya carriles duplicados dentro de la misma serie
            var agrupadoPorSerieCheck = placements.GroupBy(p => p.Serie);
            foreach (var grupo in agrupadoPorSerieCheck)
            {
                var carrilesRepetidos = grupo.GroupBy(p => p.Carril)
                                            .Where(g => g.Count() > 1)
                                            .Select(g => g.Key)
                                            .ToList();
                if (carrilesRepetidos.Any())
                {
                    throw new ArgumentException($"El carril {carrilesRepetidos.First()} estÃ¡ repetido en la Serie {grupo.Key}.");
                }
            }

            // 1. LIMPIEZA TOTAL
            await _etapaRepository.DeleteByEventoPruebaIdAsync(eventoPruebaId);

            var ep = await _eventoRepository.GetEventoPruebaByIdAsync(eventoPruebaId);
            DateTime nextTime;
            if (ep?.FechaHora != null)
            {
                nextTime = ep.FechaHora;
            }
            else
            {
                var baseDate = ep?.Evento?.Fecha.Date ?? DateTime.UtcNow.Date;
                var horaBase = ep?.Evento?.HoraInicioEvento ?? new TimeSpan(8, 0, 0);
                try
                {
                    var tzId = ep?.Evento?.TimeZoneId ?? "America/Argentina/Buenos_Aires";
                    var tz = TimeZoneInfo.FindSystemTimeZoneById(tzId);
                    var localDateTime = DateTime.SpecifyKind(baseDate.Add(horaBase), DateTimeKind.Unspecified);
                    nextTime = TimeZoneInfo.ConvertTimeToUtc(localDateTime, tz);
                }
                catch
                {
                    nextTime = DateTime.SpecifyKind(baseDate.Add(horaBase), DateTimeKind.Utc);
                }
            }

            // Asignar Plan de ProgresiÃ³n AutomÃ¡ticamente
            if (ep != null)
            {
                int inscriptosCount = placements.Count;
                ep.PlanProgresionAsignado = DeterminarPlanProgresion(inscriptosCount);
                await _eventoRepository.UpdateEventoPruebaAsync(ep);
            }

            // Determinar cuÃ¡ntas series hay
            var numSeries = placements.Max(p => p.Serie);

            // Crear Etapa de Eliminatorias (o Finales si es solo 1 serie)
            var etapaElim = new Etapa
            {
                EventoPruebaId = eventoPruebaId,
                Nombre = numSeries <= 1 ? "Finales" : "Eliminatorias",
                Tipo = numSeries <= 1 ? SportTrack_Sigdef.Entidades.Enums.TipoEtapaEnum.Final : SportTrack_Sigdef.Entidades.Enums.TipoEtapaEnum.Eliminatoria,
                Orden = numSeries <= 1 ? 3 : 1
            };
            await _etapaRepository.CreateAsync(etapaElim);

            // Agrupar placements por serie
            var agrupadoPorSerie = placements.GroupBy(p => p.Serie).OrderBy(g => g.Key);

            foreach (var grupo in agrupadoPorSerie)
            {
                var nroSerie = grupo.Key;
                var fase = new Entidades.Entidades.Fase
                {
                    EtapaId = etapaElim.Id,
                    NombreFase = numSeries <= 1 ? "Final A" : $"Serie {nroSerie}",
                    NumeroFase = nroSerie,
                    Estado = "Programada",
                    FechaHoraProgramada = nextTime
                };

                foreach (var p in grupo)
                {
                    fase.Resultados.Add(new Entidades.Entidades.Resultado
                    {
                        InscripcionId = p.InscripcionId,
                        Carril = p.Carril,
                        Estado = SportTrack_Sigdef.Entidades.Enums.EstadoResultadoEnum.Pendiente
                    });
                }

                await _faseRepository.CreateAsync(fase);
                nextTime = nextTime.AddMinutes(10);
            }

            // Si hay mÃ¡s de una serie, pre-generamos las siguientes etapas vacÃ­as (SF/Finales) como en el modo auto
            if (numSeries > 1)
            {
                var inscriptosCount = placements.Count;
                await PreGenerarSiguientesEtapasAsync(eventoPruebaId, inscriptosCount, numSeries, nextTime);
            }

            return await GetFasesPorEventoPruebaAsync(eventoPruebaId);
        }

        public async Task UpdateResultadoStatusAsync(int resultadoId, string status)
        {
            var res = await _faseRepository.GetResultadoByIdAsync(resultadoId);
            if (res == null) return;

            // Mapeo de strings (DNS, DNF, DSQ) al enum
            if (Enum.TryParse<SportTrack_Sigdef.Entidades.Enums.EstadoResultadoEnum>(status, true, out var result))
            {
                res.Estado = result;
            }
            else if (status.ToUpper() == "PENDIENTE")
            {
                res.Estado = SportTrack_Sigdef.Entidades.Enums.EstadoResultadoEnum.Pendiente;
            }
            else if (status.ToUpper() == "DSQ")
            {
                res.Estado = SportTrack_Sigdef.Entidades.Enums.EstadoResultadoEnum.Descalificado;
            }

            await _faseRepository.UpdateResultadoAsync(res);
            
            // Notificar Globalmente
            await _hubContext.Clients.All.SendAsync("GlobalResultStatusUpdated", resultadoId, status);
        }

    }
}
