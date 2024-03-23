using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Pushmonolith.ControlPlane.Processor
{
    internal class ProcessorBackgroundService : BackgroundService
    {
        private readonly ILogger<ProcessorBackgroundService> logger;
        private readonly string volumeLocation;
        private readonly SemaphoreSlim semaphore;

        private static int initConcurrency = 1;
        private static int maxConcurrency = 1;

        public ProcessorBackgroundService(
            ILogger<ProcessorBackgroundService> logger,
            IConfiguration configuration)
        {
            this.logger = logger;
            volumeLocation = configuration.GetSection("VolumeLocation").Value;
            semaphore = new SemaphoreSlim(initConcurrency, maxConcurrency);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            logger.LogDebug("Starting...");
            await Task.Yield();

            try
            {
                while (true)
                {
                    await semaphore.WaitAsync(stoppingToken).ConfigureAwait(false);
                    await ReadMessage();
                    HandleMessage();
                }
            }
            catch (OperationCanceledException)
            {
                logger.LogInformation("Cancellation requested. Stopping project processor service.");
            }
            finally
            {

            }
        }

        private async Task ReadMessage()
        {
            throw new NotImplementedException();
        }

        private void HandleMessage()
        {
            throw new NotImplementedException();
        }
    }
}
