
using Microsoft.Extensions.Caching.Memory;

namespace PruebaTecnica.Test.Api.Helpers
{
    public class TokenMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;
        private readonly IMemoryCache _memoryCache;
        private readonly ILogger<TokenMiddleware> _logger;
        private static readonly SemaphoreSlim _bankSemaphore = new SemaphoreSlim(1, 1);

        public TokenMiddleware(RequestDelegate next, IConfiguration configuration, IMemoryCache cache, ILogger<TokenMiddleware> logger)
        {
            _next = next;
            _configuration = configuration;
            _memoryCache = cache;
            _logger = logger;
        }

    }
}