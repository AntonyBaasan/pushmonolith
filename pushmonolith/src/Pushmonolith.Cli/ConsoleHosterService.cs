using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.CommandLine;
using System.Net.Http.Headers;

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
            CreateLoginCommand(),
            CreateInitCommand(),
            CreateUploadCommand()
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
    static Command CreateInitCommand()
    {
        var command = new Command("init", "");
        command.SetHandler(() =>
        {
            // read yaml config file
            Console.WriteLine("reading pushmonolith.yaml file");

            // read publish folder
            Console.WriteLine("publish folder");

            // archive publish folder
            Console.WriteLine("publish folder");

            // upload archive file
            Console.WriteLine("Logging in...");
        });
        return command;
    }

    static Command CreateUploadCommand()
    {
        var command = new Command("upload", "Deploy target application based on monolith.yaml file.");
        command.SetHandler(async () =>
        {
            // read yaml config file
            Console.WriteLine("* Reading pushmonolith.yaml file...");

            // read publish folder
            Console.WriteLine("* Reading publish directory...");

            // archive publish folder
            Console.WriteLine("* Archiving publish directory...");

            // upload archive file
            Console.WriteLine("* Uploading file...");

            var file = new FileInfo("C:\\temp\\publisih\\archive.zip");
            var client = new HttpClient();

            var content = new MultipartFormDataContent();
            var fileContent = new StreamContent(file.OpenRead());
            fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/zip");
            content.Add(fileContent, "file", file.Name);
            var response = await client.PostAsync("http://localhost:5249/api/project/1/upload", content);

            Console.WriteLine("Done.");


        });
        return command;
    }
}