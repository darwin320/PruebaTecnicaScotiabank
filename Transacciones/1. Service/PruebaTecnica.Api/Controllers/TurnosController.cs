using PruebaTecnica.Test.Entidades.Api;
using PruebaTecnica.Test.Entidades;
using Microsoft.AspNetCore.Mvc;
using PruebaTecnica.Test.Bll.Core;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using PruebaTecnica.Test.API.Helpers;
using PruebaTecnica.Test.Bll;

namespace PruebaTecnica.Test.Api.Controllers
{
    /// <summary>
    /// Controlador para la generación de turnos.
    /// </summary>
    [ApiController]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class TurnosController : Controller
    {
        private readonly ILogger<TurnosController> _logger;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Constructor del controlador.
        /// </summary>
        /// <param name="logger">Logger de la aplicación.</param>
        /// <param name="configuration">Configuración de la aplicación.</param>
        public TurnosController(ILogger<TurnosController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        /// <summary>
        /// Genera turnos en la base de datos según los parámetros proporcionados.
        /// </summary>
        /// <param name="request">Parámetros para la generación de turnos.</param>
        /// <returns>Lista de turnos generados.</returns>
        /// <response code="200">Arreglo con la lista de turnos generados.</response>
        /// <response code="400">Mensajes de error generados.</response>
        /// <response code="401">Mensajes de error generados.</response>
        /// <response code="500">Mensaje de error generado.</response>
        [HttpPost]
        [Route("api/[controller]/generarturnos")]
        [ProducesResponseType(typeof(ApiRespuestaTurnos), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiRespuestaError), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiRespuestaError), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiRespuestaError), StatusCodes.Status500InternalServerError)]
        public IActionResult GenerarTurnos([FromBody] GenerarTurnosRequest request)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    GestorTurnos gestorTurnos = new GestorTurnos(_configuration, _logger, new TurnosBll(_configuration.GetValue<string>("ConnectionStrings:DefaultConnection")));
                    List<Turno> turnosGenerados = gestorTurnos.GenerarTurnos(request.FechaInicio, request.FechaFin, request.IdServicio);
                    ApiRespuestaTurnos respuesta = new ApiRespuestaTurnos() { Turnos = turnosGenerados };
                    return Ok(respuesta);
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, ModelState.RetornarErrores(_logger));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al generar turnos");
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiRespuestaError { mensajes = new string[] { ex.Message } });
            }
        }
    }

    public class GenerarTurnosRequest
    {
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public int IdServicio { get; set; }
    }
}
