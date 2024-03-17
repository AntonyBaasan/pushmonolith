using Microsoft.AspNetCore.Http;
using Pushmonolith.Project.Models;

namespace Pushmonolith.Project.Services
{
    public interface IProjectService
    {
        Task<IList<ProjectMetadata>> GetAll();
        Task<string> Create(ProjectMetadata item);
        Task<ProjectMetadata> GetById(string id);
        Task<string> Upload(string id, IFormFile file);
    }
}