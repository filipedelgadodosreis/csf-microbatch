using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SutHubBatch.Interfaces;
using SutHubBatch.Model;
using System;
using System.IO;

namespace SutHubBatch
{
    public class MotorBusca
    {
        public readonly ISftp _sftp;
        private readonly ILogger _logger;

        public MotorBusca(ISftp sftp, ILogger<MotorBusca> logger)
        {
            _sftp = sftp;
            _logger = logger;
        }

        internal void Run()
        {
            _logger.LogInformation("[Run] Inicio da Execução {dateTime}", DateTime.UtcNow);

            try
            {
                var _configurations = BuscarConfiguracoes();
                _sftp.Download(_configurations);

            }
            catch (Exception ex)
            {
                _logger.LogError($"[Run] Erro ao efetuar do download do arquivo. {ex.Message}");
            }

            _logger.LogInformation("[Run] Fim da Execução {dateTime}", DateTime.UtcNow);
        }

        private ServiceConfigurations BuscarConfiguracoes()
        {
            ServiceConfigurations serviceConfigurations = new ServiceConfigurations();

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"appsettings.json");

            var configuration = builder.Build();

            new ConfigureFromConfigurationOptions<ServiceConfigurations>(
              configuration.GetSection("ServiceConfigurations"))
                  .Configure(serviceConfigurations);

            return serviceConfigurations;
        }
    }
}
