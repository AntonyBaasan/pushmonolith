using Pushmonolith.Store.Models;

namespace Pushmonolith.Project.Models
{
    public record ProjectMetadata : AbstractItem
    { 
        public string Name { get; init; }
    }
}
