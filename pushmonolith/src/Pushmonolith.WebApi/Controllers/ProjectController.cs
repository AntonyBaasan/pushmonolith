using Microsoft.AspNetCore.Mvc;
using Pushmonolith.Project.Models;
using Pushmonolith.Project.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Pushmonolith.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService projectService;
        
        public ProjectController(IProjectService projectService)
        {
            this.projectService = projectService;
        }

        // GET: api/<ProjectController>
        [HttpGet]
        public async Task<IEnumerable<ProjectMetadata>> Get()
        {
            return await projectService.GetAll();
        }

        // GET api/<ProjectController>/5
        [HttpGet("{id}")]
        public async Task<ProjectMetadata> Get(string id)
        {
            return await projectService.GetById(id);
        }

        // POST api/<ProjectController>
        [HttpPost]
        public async Task<ProjectMetadata> Post([FromBody] ProjectMetadata project)
        {
            await projectService.Create(project);
            return project;
        }

        // PUT api/<ProjectController>/5
        [HttpPut("{id}")]
        public void Put(string id, [FromBody] string value)
        {
        }

        // DELETE api/<ProjectController>/5
        [HttpDelete("{id}")]
        public void Delete(string id)
        {
        }

        // POST api/<ProjectController>/5/upload
        [HttpPost("{id}/upload")]
        public async Task<IActionResult> Upload(string id)
        {
            try
            {
                var formCollection = await Request.ReadFormAsync();
                var file = formCollection.Files.First();
                return Ok(await projectService.Upload(id, file));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
