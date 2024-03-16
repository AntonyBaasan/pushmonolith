using Pushmonolith.Project.Models;
using Pushmonolith.Store.Services;

namespace Pushmonolith.Project.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IPushmonolithStore store;

        public ProjectService(IPushmonolithStore store)
        {
            this.store = store;
        }

        public Task<string> Create(ProjectMetadata item)
        {
            return store.Create(item);
        }

        public Task<IList<ProjectMetadata>> GetAll()
        {
            return store.GetAll<ProjectMetadata>();
        }

        public Task<ProjectMetadata> GetById(string id)
        {
            return store.GetById<ProjectMetadata>(id);
        }
    }
}
