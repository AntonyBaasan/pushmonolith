
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Pushmonolith.Cli.ExecutionManager.Services;
using System.CommandLine;

class Program
{
    static async Task Main(string[] args)
    {
        await Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                SetServices(services, hostContext.Configuration);
                services.AddSingleton<InputArgs>(provider => new InputArgs { Args = args });
                services.AddHostedService<ConsoleHosterService>();
            })
            .RunConsoleAsync();
    }
    static async Task Main2(string[] args)
    {
        //builder.Configuration.Sources.Clear();
        //// load config
        //IConfiguration config = new ConfigurationBuilder()
        //    .AddJson("appsettings.json")
        //    //.AddJsonFile($"appsettings.{env.EnvironmentName}.json")
        //    .AddEnvirionmentVariables()
        //    .Build();

        // Setup depencency injection
        var services = new ServiceCollection();



    }
    static void SetServices(IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddSingleton<IExecutionManager, DemoExecutionManger>()
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
class InputArgs
{
    public string[] Args { get; set; }
}

