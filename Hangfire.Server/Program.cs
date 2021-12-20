using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Hangfire.Server
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var connString = "localhost:6379";
            var hostBuilder = new HostBuilder()
                .ConfigureLogging(l => { })
                .ConfigureServices((hostContext, services) =>
                {
                    connString = hostContext.Configuration.GetConnectionString("Redis");
                });

            GlobalConfiguration.Configuration.UseSerilogLogProvider();
            GlobalConfiguration.Configuration.UseRedisStorage(connString);

            using var server = new BackgroundJobServer(new BackgroundJobServerOptions()
            {
                WorkerCount = 1
            });
            await hostBuilder.RunConsoleAsync();
        }
    }
}