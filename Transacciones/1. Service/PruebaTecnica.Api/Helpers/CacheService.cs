using PruebaTecnica.Test.Bll.Utils;
using Microsoft.Extensions.Caching.Memory;

namespace PruebaTecnica.Test.API.Helpers
{
    /// <summary>
    /// Clase para el manejo del cache
    /// </summary>
    public class CacheService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _configuration;
        private readonly ILogger<CacheService> _logger; 
        
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="configuration"></param>
        /// <param name="logger"></param>
        public CacheService(IServiceProvider serviceProvider, IConfiguration configuration, ILogger<CacheService> logger)
        {
            _serviceProvider = serviceProvider;
            _configuration = configuration;
            _logger = logger;
        }

        /// <summary>
        /// Inicializar el servicio
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    IMemoryCache? cache = _serviceProvider.GetService<IMemoryCache>();
                    ExtesionesApi.CargarListaClientes(cache, _configuration, _logger);
                }
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.ExcepcionInterna(), "Error Cargando Lista Clientes Cache: {mensaje}", ex.Message);
            }
            return Task.CompletedTask;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
