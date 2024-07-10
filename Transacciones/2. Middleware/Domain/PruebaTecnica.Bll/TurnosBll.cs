using PruebaTecnica.Test.DAL;
using PruebaTecnica.Test.DAL.Interfaces;
using PruebaTecnica.Test.Entidades;
using System;
using System.Collections.Generic;

namespace PruebaTecnica.Test.Bll
{
    public class TurnosBll
    {
        #region Propiedades
        private string ConnectionString { get; set; }

        private readonly ITurnosRepository _turnosRepository;
        #endregion Propiedades

        public TurnosBll(string connectionString)
        {
            this.ConnectionString = connectionString;
            this._turnosRepository = new TurnosRepository(this.ConnectionString);
        }

        /// <summary>
        /// Permite generar y obtener los turnos desde la base de datos según los parámetros proporcionados.
        /// </summary>
        /// <param name="fechaInicio">Fecha de inicio para la generación de turnos</param>
        /// <param name="fechaFin">Fecha de fin para la generación de turnos</param>
        /// <param name="idServicio">ID del servicio para el cual se generarán los turnos</param>
        /// <returns>Lista de turnos generados</returns>
        public List<Turno> GenerarTurnos(DateTime fechaInicio, DateTime fechaFin, int idServicio)
        {
            return this._turnosRepository.GenerarTurnos(fechaInicio, fechaFin, idServicio);
        }
    }
}
