using PruebaTecnica.Test.Bll;
using PruebaTecnica.Test.Bll.Utils;
using PruebaTecnica.Test.Entidades;
using PruebaTecnica.Test.Entidades.Api;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace PruebaTecnica.Test.API.Helpers
{
    /// <summary>
    /// 
    /// </summary>
    public static class ExtesionesApi
    {
        public const string ListaCliente = "ListaClientes";
        public const string Cliente = "Cliente";

        /// <summary>
        /// formateo de error no controlado
        /// </summary>
        /// <param name="error"></param>
        /// <param name="logger"></param>
        /// <returns>ApiRespuestaError</returns>
        public static ApiRespuestaError RetornarErrores(this Exception error, ILogger logger)
        {
            ApiRespuestaError objRetorno = new ApiRespuestaError();
            Exception current = error.ExcepcionInterna();
            string msg = "Un error no controlado no permite completar la solicitud. Por favor intente más tarde.";
            if (current is PPException)
            {
                PPException ppException = (PPException)current;
                msg = ppException.Message;
                logger.LogError(ppException, "Error no controlado #: {traza}", ppException.mensajelog);
            }
            else
            {
                logger.LogError(current, "Error no controlado: {traza}", current.StackTrace);
            }
            objRetorno.mensajes = new string[] { msg };
            return objRetorno;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelstate"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
        public static ApiRespuestaError RetornarErrores(this ModelStateDictionary modelstate, ILogger logger)
        {
            ApiRespuestaError objRetorno = new ApiRespuestaError
            {
                mensajes = modelstate.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToArray()
            };
            logger.LogWarning("Error Datos Entrada: {errores}", JsonConvert.SerializeObject(objRetorno.mensajes));
            objRetorno.mensajes = new string[] { new("Un error en los datos de entrada no permite completar la solicitud. Por favor verifique los datos.") };
            return objRetorno;
        }

        /// <summary>
        /// carga lista clientes en cache
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="configuration"></param>
        public static void CargarListaClientes(this IMemoryCache cache, IConfiguration configuration, ILogger logger)
        {
            ClienteBll clienteBll = new ClienteBll(configuration.GetValue<string>("ConnectionStrings:DefaultConnection"));
            List<Cliente> lstclientes = clienteBll.GetClientes();
            int minutoscache = configuration.GetValue<int>("Configuraciones:MinutosCache");
            cache.Set(ListaCliente, lstclientes, DateTime.Now.AddMinutes(minutoscache));
            logger.LogInformation("Se cargaron {conteo} clientes", lstclientes.Count);
        }

    }
}
