namespace Pushmonolith.Cli.ExecutionManager.Models
{
    public class ExecutionCommand
    {
        public string Command { get; set; }
        public IList<string> Arguments { get; set; }
    }
}