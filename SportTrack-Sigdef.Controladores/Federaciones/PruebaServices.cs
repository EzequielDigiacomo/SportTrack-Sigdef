using Microsoft.EntityFrameworkCore;
using SportTrack_Sigdef.AccesoDatos;
using SportTrack_Sigdef.Entidades.Entidades;
using SportTrack_Sigdef.Entidades.Enums;
using SportTrack_Sigdef.Entidades.DTOs.Prueba;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SportTrack_Sigdef.Controladores.Federaciones;
using Microsoft.AspNetCore.Mvc;

namespace SportTrack_Sigdef.Controladores.Services
{
    public class PruebaServices : IPruebaServices
    {
        private readonly SportTrackDbContext _context;

        public PruebaServices(SportTrackDbContext context)
        {
            _context = context;
        }

        public async Task<ActionResult<IEnumerable<PruebaDto>>> GetPruebas()
        {
            try
            {
                var rawPruebas = await _context.Pruebas.ToListAsync();
                var pruebas = rawPruebas.Select(p => new PruebaDto
                {
                    IdPrueba = p.IdPrueba,
                    Distancia = MapDistanciaToEnum(p.DistanciaId),
                    CategoriaEdad = (CategoriaEdad)p.CategoriaEdad,
                    SexoCompetencia = (SexoCompetencia)(p.SexoCompetencia - 1),
                    TipoBote = (TipoBote)(p.TipoBote - 1)
                }).ToList();

                return new OkObjectResult(pruebas);
            }
            catch (Exception ex)
            {
                return new StatusCodeResult(500);
            }
        }

        public async Task<ActionResult<PruebaDto>> GetPrueba(int id)
        {
            try
            {
                var prueba = await _context.Pruebas.FindAsync(id);

                if (prueba == null)
                {
                    return new NotFoundResult();
                }

                return new OkObjectResult(new PruebaDto
                {
                    IdPrueba = prueba.IdPrueba,
                    Distancia = MapDistanciaToEnum(prueba.DistanciaId),
                    CategoriaEdad = (CategoriaEdad)prueba.CategoriaEdad,
                    SexoCompetencia = (SexoCompetencia)(prueba.SexoCompetencia - 1),
                    TipoBote = (TipoBote)(prueba.TipoBote - 1)
                });
            }
            catch (Exception ex)
            {
                return new StatusCodeResult(500);
            }
        }

        public async Task<ActionResult<PruebaDto>> PostPrueba(PruebaCreateDto pruebaDto)
        {
            try
            {
                int dbSexoId = (int)pruebaDto.SexoCompetencia + 1;
                int dbBoteId = (int)pruebaDto.TipoBote + 1;
                int dbCategoriaId = MapEnumToCategoriaId(pruebaDto.CategoriaEdad);
                int dbDistanciaId = MapEnumToDistanciaId(pruebaDto.Distancia);

                var existe = await _context.Pruebas.AnyAsync(p =>
                    p.DistanciaId == dbDistanciaId &&
                    p.CategoriaEdad == dbCategoriaId &&
                    p.SexoCompetencia == dbSexoId &&
                    p.TipoBote == dbBoteId);

                if (existe)
                {
                    return new BadRequestResult();
                }

                var prueba = new Prueba
                {
                    DistanciaId = dbDistanciaId,
                    CategoriaEdad = dbCategoriaId,
                    SexoCompetencia = dbSexoId,
                    TipoBote = dbBoteId,
                    Nombre = $"{pruebaDto.CategoriaEdad} {pruebaDto.TipoBote} {pruebaDto.Distancia} {pruebaDto.SexoCompetencia}"
                };

                _context.Pruebas.Add(prueba);
                await _context.SaveChangesAsync();

                var resultDto = new PruebaDto
                {
                    IdPrueba = prueba.IdPrueba,
                    Distancia = MapDistanciaToEnum(prueba.DistanciaId),
                    CategoriaEdad = (CategoriaEdad)prueba.CategoriaEdad,
                    SexoCompetencia = (SexoCompetencia)(prueba.SexoCompetencia - 1),
                    TipoBote = (TipoBote)(prueba.TipoBote - 1)
                };

                var result = new ObjectResult(resultDto)
                {
                    StatusCode = 201
                };
                return result;
            }
            catch (DbUpdateException dbEx)
            {
                return new StatusCodeResult(500);
            }
            catch (Exception ex)
            {
                return new StatusCodeResult(500);
            }
        }

        public async Task<IActionResult> PutPrueba(int id, PruebaCreateDto pruebaDto)
        {
            try
            {
                var prueba = await _context.Pruebas.FindAsync(id);

                if (prueba == null)
                {
                    return new NotFoundResult();
                }

                int dbSexoId = (int)pruebaDto.SexoCompetencia + 1;
                int dbBoteId = (int)pruebaDto.TipoBote + 1;
                int dbCategoriaId = MapEnumToCategoriaId(pruebaDto.CategoriaEdad);
                int dbDistanciaId = MapEnumToDistanciaId(pruebaDto.Distancia);

                prueba.DistanciaId = dbDistanciaId;
                prueba.CategoriaEdad = dbCategoriaId;
                prueba.SexoCompetencia = dbSexoId;
                prueba.TipoBote = dbBoteId;

                await _context.SaveChangesAsync();
                return new NoContentResult();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await PruebaExistsAsync(id))
                {
                    return new NotFoundResult();
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                return new StatusCodeResult(500);
            }
        }

        public async Task<IActionResult> DeletePrueba(int id)
        {
            try
            {
                var prueba = await _context.Pruebas.FindAsync(id);
                if (prueba == null)
                {
                    return new NotFoundResult();
                }

                _context.Pruebas.Remove(prueba);
                await _context.SaveChangesAsync();

                return new NoContentResult();
            }
            catch (DbUpdateException dbEx)
            {
                return new StatusCodeResult(500);
            }
            catch (Exception ex)
            {
                return new StatusCodeResult(500);
            }
        }

        private async Task<bool> PruebaExistsAsync(int id)
        {
            return await _context.Pruebas.AnyAsync(e => e.IdPrueba == id);
        }

        private static DistanciaRegata MapDistanciaToEnum(int distanciaId)
        {
            return distanciaId switch
            {
                5 => DistanciaRegata.QuinientosMetros,
                6 => DistanciaRegata.MilMetros,
                8 => DistanciaRegata.DosKilometros,
                9 => DistanciaRegata.TresKilometros,
                10 => DistanciaRegata.CincoKilometros,
                11 => DistanciaRegata.DiezKilometros,
                _ => (DistanciaRegata)distanciaId
            };
        }

        private static int MapEnumToDistanciaId(DistanciaRegata distancia)
        {
            return distancia switch
            {
                DistanciaRegata.QuinientosMetros => 5,
                DistanciaRegata.MilMetros => 6,
                DistanciaRegata.DosKilometros => 8,
                DistanciaRegata.TresKilometros => 9,
                DistanciaRegata.CincoKilometros => 10,
                DistanciaRegata.DiezKilometros => 11,
                _ => (int)distancia
            };
        }

        private static int MapEnumToCategoriaId(CategoriaEdad cat)
        {
            return cat switch
            {
                CategoriaEdad.Sub23 => 6,
                CategoriaEdad.Senior => 7,
                CategoriaEdad.MasterA => 8,
                _ => (int)cat
            };
        }
    }
}

