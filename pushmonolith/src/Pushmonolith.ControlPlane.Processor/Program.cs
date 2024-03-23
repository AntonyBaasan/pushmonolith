using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using dotenv.net;
using Pushmonolith.ControlPlane.Processor;

class Program
{
    static async Task Main(string[] args)
    {
        DotEnv.Load();
        await Host.CreateDefaultBuilder(args)
            .ConfigureHostConfiguration(configHost =>
            {
                var environmentName = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");
                configHost.AddJsonFile("appsettings.json", optional: true);
                configHost.AddJsonFile($"appsettings.{environmentName}.json", optional: true);
                configHost.AddCommandLine(args);
            })
            .ConfigureServices((hostContext, services) =>
            {
                SetServices(services, hostContext.Configuration);
                services.AddHostedService<ProcessorBackgroundService>();
            })
            .RunConsoleAsync();
    }
    static void SetServices(IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddLogging(config =>
            {
                config.ClearProviders();
                config.AddConfiguration(configuration.GetSection("Logging"));
                config.AddFilter<ConsoleLoggerProvider>("Microsoft.Hosting.Lifetime", LogLevel.Error);

                //config.AddDebug();
                //config.AddEventSourceLogger();
                config.AddConsole();
            })
            .BuildServiceProvider();
    }
}

