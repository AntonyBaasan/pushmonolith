using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.CommandLine;

internal class ConsoleHosterService : IHostedService
{
    private readonly ILogger<ConsoleHosterService> logger;
    private readonly IHostApplicationLifetime applicationLifetime;
    private readonly InputArgs inputArgs;

    public ConsoleHosterService(
        ILogger<ConsoleHosterService> logger, 
        IHostApplicationLifetime applicationLifetime,
        InputArgs inputArgs)
    {
        this.logger = logger;
        this.applicationLifetime = applicationLifetime;
        this.inputArgs = inputArgs;
    }
    public Task StartAsync(CancellationToken cancellationToken)
    {
        logger.LogDebug("Starting...");

        applicationLifetime.ApplicationStarted.Register(() =>
        {
            Task.Run(async () =>
            {
                try
                {
                    var rootCommand = new RootCommand("Sample command-line app");
                    foreach (var command in GetCommands())
                    {
                        rootCommand.AddCommand(command);
                    }
                    rootCommand.SetHandler(() => Console.WriteLine("Type 'pushmonolith --help' for more information"));

                    await rootCommand.InvokeAsync(inputArgs.Args);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Unhandled exception!");
                }
                finally
                {
                    // Stop the application once the work is done
                    applicationLifetime.StopApplication();
                }
            });
        });

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    static List<Command> GetCommands()
    {
        return
        [
            CreateDebugCommand(),
            CreateLoginCommand()
        ];
    }

    static Command CreateDebugCommand()
    {
        var command = new Command("debug", "debug");
        command.SetHandler(() =>
        {
            Console.WriteLine("Hello world");
        });
        return command;
    }
    static Command CreateLoginCommand()
    {
        var command = new Command("login", "logs in user");
        command.SetHandler(() =>
        {
            Console.WriteLine("Logging in...");
        });
        return command;
    }
}