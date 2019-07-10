using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SutHubBatch.Interfaces;
using SutHubBatch.Service;

namespace SutHubBatch
{
    static class Program
    {
        static void Main(string[] args)
        {
            var services = new ServiceCollection();

            ConfigureServices(services);
            using (ServiceProvider serviceProvider =
                                   services.BuildServiceProvider())
            {
                MotorBusca app = serviceProvider.GetService<MotorBusca>();
                app.Run();
            }
        }

        private static void ConfigureServices(ServiceCollection services)
        {
            services.AddLogging(configure => configure.AddConsole())
                               .AddSingleton<ISftp, SftpServiceImplementation>()
                               .AddSingleton<MotorBusca>();
        }
    }
}
