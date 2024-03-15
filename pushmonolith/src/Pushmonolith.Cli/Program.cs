
using Microsoft.Extensions.DependencyInjection;
using Pushmonolith.Cli.ExecutionManager.Services;
using System.CommandLine;

class Program
{
    static async Task Main(string[] args)
    {
        // Setup depencency injection
        var services = new ServiceCollection();
        SetServices(services);

        var rootCommand = new RootCommand("Sample command-line app");
        foreach (var command in GetCommands())
        {
            rootCommand.AddCommand(command);
        }
        rootCommand.SetHandler(() => Console.WriteLine("Type 'pushmonolith --help' for more information"));

        await rootCommand.InvokeAsync(args);
    }
    static void SetServices(ServiceCollection services)
    {
        services
            .AddSingleton<IExecutionManager, DemoExecutionManger>()
            .BuildServiceProvider();
    }

    static List<Command> GetCommands()
    {
        return new List<Command>
        {
            CreateDebugCommand(),
            CreateLoginCommand()
        };
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

