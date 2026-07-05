using SportTrack_Sigdef.Entidades.Enums;
﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace SportTrack_Sigdef.Entidades.DTOs.Evento
{
    // ðŸ”¹ DTO PARA PRUEBAS DEL EVENTO (RESPUESTA)
    public class EventoPruebaResponseDto
    {
        public int IdEventoPrueba { get; set; }
        public int DistanciaId { get; set; }
        public string DistanciaCodigo { get; set; } = string.Empty;
        public string DistanciaNombre { get; set; } = string.Empty;
        public decimal Metros { get; set; }
        public int CategoriaEdad { get; set; }
        public decimal? PrecioCategoria { get; set; }
        public int DistanciaRegata { get; set; }
        public int TipoBote { get; set; }
        public string TipoBoteNombre { get; set; } = string.Empty;
        public int SexoCompetencia { get; set; }

        private static SportTrack_Sigdef.Entidades.Enums.DistanciaRegata MapToDistanciaRegata(DistanciaRegataEnum enumVal)
        {
            return enumVal switch
            {
                DistanciaRegataEnum.Metros200 => SportTrack_Sigdef.Entidades.Enums.DistanciaRegata.DoscientosMetros,
                DistanciaRegataEnum.Metros350 => SportTrack_Sigdef.Entidades.Enums.DistanciaRegata.TrecientosCincuentaMetros,
                DistanciaRegataEnum.Metros400 => SportTrack_Sigdef.Entidades.Enums.DistanciaRegata.QuatroCientosMetros,
                DistanciaRegataEnum.Metros500 => SportTrack_Sigdef.Entidades.Enums.DistanciaRegata.QuinientosMetros,
                DistanciaRegataEnum.Metros1000 => SportTrack_Sigdef.Entidades.Enums.DistanciaRegata.MilMetros,
                DistanciaRegataEnum.Metros2000 => SportTrack_Sigdef.Entidades.Enums.DistanciaRegata.DosKilometros,
                DistanciaRegataEnum.Metros3000 => SportTrack_Sigdef.Entidades.Enums.DistanciaRegata.TresKilometros,
                DistanciaRegataEnum.Metros5000 => SportTrack_Sigdef.Entidades.Enums.DistanciaRegata.CincoKilometros,
                DistanciaRegataEnum.Metros10000 => SportTrack_Sigdef.Entidades.Enums.DistanciaRegata.DiezKilometros,
                DistanciaRegataEnum.Metros15000 => SportTrack_Sigdef.Entidades.Enums.DistanciaRegata.QuinceKilometros,
                DistanciaRegataEnum.Metros22000 => SportTrack_Sigdef.Entidades.Enums.DistanciaRegata.VeintiDosKilometros,
                _ => SportTrack_Sigdef.Entidades.Enums.DistanciaRegata.MilMetros
            };
        }

        public static EventoPruebaResponseDto FromEntity(Entidades.EventoPrueba eventoPrueba)
        {
            if (eventoPrueba?.Prueba == null || eventoPrueba.Prueba.Distancia == null) return new EventoPruebaResponseDto();

            var distRegata = MapToDistanciaRegata(eventoPrueba.Prueba.Distancia.DistanciaRegata);
            return new EventoPruebaResponseDto
            {
                IdEventoPrueba = eventoPrueba.IdEventoPrueba,
                DistanciaId = (int)distRegata,
                DistanciaCodigo = GetDistanciaDisplay(distRegata),
                DistanciaNombre = distRegata.ToString(),
                Metros = GetDistanciaMetros(distRegata),
                CategoriaEdad = (int)eventoPrueba.Prueba.CategoriaEdad,
                PrecioCategoria = eventoPrueba.PrecioCategoria,
                TipoBote = (int)eventoPrueba.Prueba.TipoBote,
                TipoBoteNombre = eventoPrueba.Prueba.TipoBote.ToString(),
                SexoCompetencia = (int)eventoPrueba.Prueba.SexoCompetencia,
                DistanciaRegata = (int)distRegata
            };
        }

        private static string GetDistanciaDisplay(SportTrack_Sigdef.Entidades.Enums.DistanciaRegata distancia)
        {
            return distancia switch
            {
                SportTrack_Sigdef.Entidades.Enums.DistanciaRegata.DoscientosMetros => "200m",
                SportTrack_Sigdef.Entidades.Enums.DistanciaRegata.TrecientosCincuentaMetros => "350m",
                SportTrack_Sigdef.Entidades.Enums.DistanciaRegata.QuatroCientosMetros => "400m",
                SportTrack_Sigdef.Entidades.Enums.DistanciaRegata.QuinientosMetros => "500m",
                SportTrack_Sigdef.Entidades.Enums.DistanciaRegata.MilMetros => "1000m",
                SportTrack_Sigdef.Entidades.Enums.DistanciaRegata.DosKilometros => "2K",
                SportTrack_Sigdef.Entidades.Enums.DistanciaRegata.TresKilometros => "3K",
                SportTrack_Sigdef.Entidades.Enums.DistanciaRegata.CincoKilometros => "5K",
                SportTrack_Sigdef.Entidades.Enums.DistanciaRegata.DiezKilometros => "10K",
                SportTrack_Sigdef.Entidades.Enums.DistanciaRegata.QuinceKilometros => "15K",
                SportTrack_Sigdef.Entidades.Enums.DistanciaRegata.VeintiDosKilometros => "22K",
                SportTrack_Sigdef.Entidades.Enums.DistanciaRegata.VeintiCincoKilometros => "25K",
                SportTrack_Sigdef.Entidades.Enums.DistanciaRegata.TreintaDosKilometros => "32K",
                _ => distancia.ToString()
            };
        }

        private static decimal GetDistanciaMetros(SportTrack_Sigdef.Entidades.Enums.DistanciaRegata distancia)
        {
            return distancia switch
            {
                SportTrack_Sigdef.Entidades.Enums.DistanciaRegata.DoscientosMetros => 200,
                SportTrack_Sigdef.Entidades.Enums.DistanciaRegata.TrecientosCincuentaMetros => 350,
                SportTrack_Sigdef.Entidades.Enums.DistanciaRegata.QuatroCientosMetros => 400,
                SportTrack_Sigdef.Entidades.Enums.DistanciaRegata.QuinientosMetros => 500,
                SportTrack_Sigdef.Entidades.Enums.DistanciaRegata.MilMetros => 1000,
                SportTrack_Sigdef.Entidades.Enums.DistanciaRegata.DosKilometros => 2000,
                SportTrack_Sigdef.Entidades.Enums.DistanciaRegata.TresKilometros => 3000,
                SportTrack_Sigdef.Entidades.Enums.DistanciaRegata.CincoKilometros => 5000,
                SportTrack_Sigdef.Entidades.Enums.DistanciaRegata.DiezKilometros => 10000,
                SportTrack_Sigdef.Entidades.Enums.DistanciaRegata.QuinceKilometros => 15000,
                SportTrack_Sigdef.Entidades.Enums.DistanciaRegata.VeintiDosKilometros => 22000,
                SportTrack_Sigdef.Entidades.Enums.DistanciaRegata.VeintiCincoKilometros => 25000,
                SportTrack_Sigdef.Entidades.Enums.DistanciaRegata.TreintaDosKilometros => 32000,
                _ => 0
            };
        }
    }
}
