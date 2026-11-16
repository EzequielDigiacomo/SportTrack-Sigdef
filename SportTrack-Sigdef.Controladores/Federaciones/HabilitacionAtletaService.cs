using System;
using System.Collections.Generic;
using System.Linq;
using SportTrack_Sigdef.Entidades.Entidades;
using SportTrack_Sigdef.Entidades.Enums;

namespace SportTrack_Sigdef.Controladores.Federaciones
{
    public class HabilitacionResultado
    {
        public bool EstaHabilitado { get; set; }
        public string EstadoHabilitacion => EstaHabilitado ? "Habilitado" : "NoHabilitado";
        public List<string> MotivosBloqueo { get; set; } = new();
        public string EstadoApto { get; set; } = "Faltante";
        public bool ChecklistDocsOk { get; set; }
        public List<string> DocsFaltantes { get; set; } = new();
    }

    public interface IHabilitacionAtletaService
    {
        HabilitacionResultado Evaluar(AtletaFederacion atleta, IEnumerable<DocumentacionFederacionPersona> docs, DateTime? ahoraUtc = null);
        List<string> ChecklistDocsFaltantes(AtletaFederacion atleta, IEnumerable<DocumentacionFederacionPersona> docs, DateTime? ahoraUtc = null);
        string EvaluarEstadoApto(AtletaFederacion atleta, IEnumerable<DocumentacionFederacionPersona> docs, DateTime? ahoraUtc = null);
        bool EsMenor(AtletaFederacion atleta, DateTime? ahoraUtc = null);
    }

    public class HabilitacionAtletaService : IHabilitacionAtletaService
    {
        private static readonly int[] TiposDni = { (int)TipoDocumentoFederacion.DniFrente, (int)TipoDocumentoFederacion.Pasaporte };
        private const int TipoApto = (int)TipoDocumentoFederacion.AptoMedico;
        private const int TipoAutorizacion = (int)TipoDocumentoFederacion.AutorizacionMenor;
        private const int DiasPorVencer = 30;

        public HabilitacionResultado Evaluar(AtletaFederacion atleta, IEnumerable<DocumentacionFederacionPersona> docs, DateTime? ahoraUtc = null)
        {
            var ahora = ahoraUtc ?? DateTime.UtcNow;
            var listaDocs = docs?.ToList() ?? new List<DocumentacionFederacionPersona>();
            var resultado = new HabilitacionResultado
            {
                EstadoApto = EvaluarEstadoApto(atleta, listaDocs, ahora),
                DocsFaltantes = ChecklistDocsFaltantes(atleta, listaDocs, ahora)
            };
            resultado.ChecklistDocsOk = resultado.DocsFaltantes.Count == 0;

            if (atleta.EstadoFichaje != EstadoFichaje.Aprobado)
                resultado.MotivosBloqueo.Add($"Fichaje: {atleta.EstadoFichaje}");

            if (atleta.EstadoPago != EstadoPago.Pagado)
                resultado.MotivosBloqueo.Add($"Pago: {atleta.EstadoPago}");

            if (resultado.EstadoApto != "Vigente")
                resultado.MotivosBloqueo.Add($"Apto: {resultado.EstadoApto}");

            if (!resultado.ChecklistDocsOk)
                resultado.MotivosBloqueo.Add("Documentación incompleta: " + string.Join(", ", resultado.DocsFaltantes));

            resultado.EstaHabilitado = resultado.MotivosBloqueo.Count == 0;
            return resultado;
        }

        public List<string> ChecklistDocsFaltantes(AtletaFederacion atleta, IEnumerable<DocumentacionFederacionPersona> docs, DateTime? ahoraUtc = null)
        {
            var lista = docs?.ToList() ?? new List<DocumentacionFederacionPersona>();
            var faltantes = new List<string>();

            var tieneDni = lista.Any(d => d.TipoDocumento.HasValue && TiposDni.Contains(d.TipoDocumento.Value));
            if (!tieneDni)
                faltantes.Add("DNI o Pasaporte");

            var tieneAptoDoc = lista.Any(d => d.TipoDocumento == TipoApto);
            if (!tieneAptoDoc && !atleta.PresentoAptoMedico)
                faltantes.Add("Apto médico");

            if (EsMenor(atleta, ahoraUtc))
            {
                var tieneAuth = lista.Any(d => d.TipoDocumento == TipoAutorizacion);
                if (!tieneAuth)
                    faltantes.Add("Autorización de menor");
            }

            return faltantes;
        }

        public string EvaluarEstadoApto(AtletaFederacion atleta, IEnumerable<DocumentacionFederacionPersona> docs, DateTime? ahoraUtc = null)
        {
            var ahora = (ahoraUtc ?? DateTime.UtcNow).Date;
            var lista = docs?.ToList() ?? new List<DocumentacionFederacionPersona>();

            DateTime? vencimiento = atleta.FechaVencimientoApto;
            var aptoDoc = lista
                .Where(d => d.TipoDocumento == TipoApto)
                .OrderByDescending(d => d.FechaVencimiento ?? d.FechaCarga)
                .FirstOrDefault();

            if (aptoDoc?.FechaVencimiento != null)
            {
                if (!vencimiento.HasValue || aptoDoc.FechaVencimiento > vencimiento)
                    vencimiento = aptoDoc.FechaVencimiento;
            }

            if (!atleta.PresentoAptoMedico && aptoDoc == null)
                return "Faltante";

            if (!vencimiento.HasValue)
                return "SinVencimientoInformado";

            var vence = vencimiento.Value.Date;
            if (vence < ahora)
                return "Vencido";
            if (vence <= ahora.AddDays(DiasPorVencer))
                return "PorVencer";
            return "Vigente";
        }

        public bool EsMenor(AtletaFederacion atleta, DateTime? ahoraUtc = null)
        {
            if (atleta.Participante == null) return false;
            var nac = atleta.Participante.FechaNacimiento;
            if (nac == default) return false;
            var ahora = ahoraUtc ?? DateTime.UtcNow;
            var edad = ahora.Year - nac.Year;
            if (ahora.DayOfYear < nac.DayOfYear) edad--;
            return edad < 18;
        }
    }
}
