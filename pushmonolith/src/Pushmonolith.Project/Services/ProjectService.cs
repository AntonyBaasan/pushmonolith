using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Pushmonolith.Project.Models;
using Pushmonolith.Store.Services;
using System.Net.Http.Headers;

namespace Pushmonolith.Project.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IPushmonolithStore store;
        private readonly string volumeLocation;

        public ProjectService(IPushmonolithStore store, IConfiguration configuration)
        {
            this.store = store;
            volumeLocation = configuration.GetSection("VolumeLocation").Value;
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

        public async Task<string> Upload(string id, IFormFile file)
        {
            if (file?.Length == 0)
            {
                return "No file passed!";
            }

            var directory = Path.Combine(volumeLocation, id);
            var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            Directory.CreateDirectory(directory);
            var fullPath = Path.Combine(directory, fileName);
            using (var stream = new FileStream(fullPath, FileMode.OpenOrCreate))
            {
                await file.CopyToAsync(stream);
            }
            return "Success";
        }
    }
}
