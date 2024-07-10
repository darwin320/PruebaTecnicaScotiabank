using PruebaTecnica.Test.Bll.Utils;
using PruebaTecnica.Test.Entidades;
using PruebaTecnica.Test.Entidades.Api;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace PruebaTecnica.Test.API.Helpers
{
    /// <summary>
    /// 
    /// </summary>
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;
        private readonly IMemoryCache _memoryCache;
        private readonly ILogger<ApiKeyMiddleware> _logger;
        private string ApiKey;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="next"></param>
        public ApiKeyMiddleware(RequestDelegate next, IConfiguration configuration, IMemoryCache cache, ILogger<ApiKeyMiddleware> logger)
        {
            _next = next;
            _configuration = configuration;
            _memoryCache = cache;
            _logger = logger;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                List<Cliente> clientes = new();
                if (!_memoryCache.TryGetValue(ExtesionesApi.ListaCliente, out clientes))
                {
                    ExtesionesApi.CargarListaClientes(_memoryCache, _configuration, _logger);
                    if (!_memoryCache.TryGetValue(ExtesionesApi.ListaCliente, out clientes))
                    {
                        _logger.LogWarning("No se pudo obtener lista clientes middleware");
                    }
                }
                ApiKey = _configuration.GetValue<string>("Configuraciones:ApiKeyName");
                if (!context.Request.Headers.TryGetValue(ApiKey, out var extractedApiKey))
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    context.Response.ContentType = "application/json";
                    string msg = "Api Key requerida";
                    await context.Response.WriteAsync(JsonConvert.SerializeObject(new ApiRespuestaError() { mensajes = new string[] { msg } }));
                    return;
                }
                if (!clientes.Exists(c => c.apikey.Equals(extractedApiKey.ToString())))
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    context.Response.ContentType = "application/json";
                    string msg = $"Cliente no autorizado: {extractedApiKey}";
                    _logger.LogWarning(msg);
                    await context.Response.WriteAsync(JsonConvert.SerializeObject(new ApiRespuestaError() { mensajes = new string[] { msg } }));
                    return;
                }
                context.Items.Add(ExtesionesApi.Cliente, clientes.Find(c => c.apikey.Equals(extractedApiKey.ToString())));
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.ExcepcionInterna(), "Error en el middleware: {mensaje}", ex.Message);
            }
            await _next(context);
        }
    }
}
