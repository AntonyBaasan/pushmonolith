using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Pushmonolith.Cli.Models;
using System.CommandLine;
using System.IO.Compression;
using System.Net.Http.Headers;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Serialization;

internal class ConsoleHosterService : IHostedService
{
    private readonly ILogger<ConsoleHosterService> logger;
    private readonly IHostApplicationLifetime applicationLifetime;
    private readonly InputArgs inputArgs;
    private string baseUrl;

    public ConsoleHosterService(
        ILogger<ConsoleHosterService> logger,
        IHostApplicationLifetime applicationLifetime,
        IConfiguration configuration,
        InputArgs inputArgs)
    {
        this.logger = logger;
        this.applicationLifetime = applicationLifetime;
        this.inputArgs = inputArgs;
        baseUrl = configuration.GetSection("Backend:BaseUrl").Value;
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

    List<Command> GetCommands()
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

    Command CreateUploadCommand()
    {
        var command = new Command("upload", "Deploy target application based on monolith.yaml file.");
        command.SetHandler(async () =>
        {
            // read yaml config file
            Console.WriteLine("* Reading pushmonolith.yaml file...");
            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(UnderscoredNamingConvention.Instance)  // see height_in_inches in sample yml 
                .WithNamingConvention(LowerCaseNamingConvention.Instance)
                .Build();
            UploadMetadata uploadMetadata = null;
            using (TextReader reader = new StreamReader("C:\\Users\\abaasandorj\\git\\pushmonolith\\pushmonolith\\src\\Pushmonolith.Cli\\pushmonolith-test.yaml"))
            {
                uploadMetadata = deserializer.Deserialize<UploadMetadata>(reader);
            }

            // archive publish folder
            Console.WriteLine("* Archiving publish directory...");
            string sourceFileName = "C:\\temp\\publisih\\archive.zip";
            File.Delete(sourceFileName);
            ZipFile.CreateFromDirectory(uploadMetadata.Directory, sourceFileName, CompressionLevel.Optimal, false);

            // upload archive file
            Console.WriteLine("* Uploading file...");
            var sourceFile = new FileInfo(sourceFileName);
            var client = new HttpClient();
            var content = new MultipartFormDataContent();
            var fileContent = new StreamContent(sourceFile.OpenRead());
            fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/zip");
            content.Add(fileContent, "file", sourceFile.Name);
            var uploadUrl = Path.Combine(baseUrl, $"api/project/{uploadMetadata.ProjectId}/upload");
            var response = await client.PostAsync(uploadUrl, content);

            Console.WriteLine("Done.");
        });
        return command;
    }
}