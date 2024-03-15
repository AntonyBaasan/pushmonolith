using Pushmonolith.Cli.ExecutionManager.Models;

namespace Pushmonolith.Cli.ExecutionManager.Services
{
    public interface IExecutionManager
    {
        public Task<ExecutionResult> ExecuteAsync(ICommand command, CancellationToken cancellationToken);
    }
}
