using Pushmonolith.Cli.ExecutionManager.Models;

namespace Pushmonolith.Cli.ExecutionManager.Services
{
    public class DemoExecutionManger : IExecutionManager
    {
        public Task<ExecutionResult> ExecuteAsync(ICommand command, CancellationToken cancellationToken)
        {
            return Task.FromResult(new ExecutionResult
            {
                IsSuccess = true,
                Output = "Demo output"
            }); 
        }
    }
}
