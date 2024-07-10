using PruebaTecnica.Test.Entidades;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace PruebaTecnica.Test.Bll.Core
{
    public class GestorTurnos
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        private readonly TurnosBll _turnosBll;

        public GestorTurnos(IConfiguration configuration, ILogger logger, TurnosBll turnosBll)
        {
            _configuration = configuration;
            _logger = logger;
            _turnosBll = turnosBll;
        }

        /// <summary>
        /// Genera y obtiene los turnos desde la base de datos según los parámetros proporcionados.
        /// </summary>
        /// <param name="fechaInicio">Fecha de inicio para la generación de turnos</param>
        /// <param name="fechaFin">Fecha de fin para la generación de turnos</param>
        /// <param name="idServicio">ID del servicio para el cual se generarán los turnos</param>
        /// <returns>Lista de turnos generados</returns>
        public List<Turno> GenerarTurnos(DateTime fechaInicio, DateTime fechaFin, int idServicio)
        {
            try
            {
                _logger.LogInformation($"Generando turnos desde {fechaInicio} hasta {fechaFin} para el servicio {idServicio}");
                return _turnosBll.GenerarTurnos(fechaInicio, fechaFin, idServicio);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al generar turnos");
                throw new ApplicationException("Ocurrió un error al generar los turnos. Por favor, inténtelo nuevamente.", ex);
            }
        }
    }
}
