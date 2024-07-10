using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PruebaTecnica.Test.Bll.Core;
using PruebaTecnica.Test.Entidades;
using NUnit.Framework;

namespace TestPruebaTecnica
{
    [TestFixture]
    public class TurnosTest
    {
        private IConfiguration _configuration;
        private ILogger<TurnosTest> _logger;

        public static IConfiguration InitConfiguration()
        {
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("appsettings.Pruebas.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
            return config;
        }

        public static ILogger<TurnosTest> InitLogger()
        {
            IServiceProvider serviceProvider = new ServiceCollection()
                .AddLogging()
                .BuildServiceProvider();
            ILoggerFactory factory = serviceProvider.GetService<ILoggerFactory>();
            ILogger<TurnosTest> logger = factory.CreateLogger<TurnosTest>();
            return logger;
        }

        [SetUp]
        public void Setup()
        {
            _configuration = InitConfiguration();
            _logger = InitLogger();
        }

        [Test]
        public void TestTurnos()
        {
            // Arrange
            Cliente cliente = new Cliente() { idcliente = 0, nombre = "testing" };
            GestorTurnos turnos = new GestorTurnos(_configuration, _logger, null);

            // Act & Assert
            Assert.Throws<Exception>(() =>
            {
                turnos.GenerarTurnos(new DateTime(), new DateTime(),1);
            });
        }
    }
}
