using System;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using ConsumerModule.GitLab.Domain;
using Microsoft.AspNetCore.Mvc;

namespace ConsumerModule.GitLab.Controllers
{
    [ApiController]
    [Route("projects")]
    public class ProjectsController : Controller
    {
        private readonly IGitLabHandler _gitLabHandler;

        public ProjectsController(IGitLabHandler gitLabHandler)
        {
            _gitLabHandler = gitLabHandler;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetProjects(int page = 0, int pageSize = 1000)
        {
            var projects = await _gitLabHandler.GetProjects(page, pageSize);

            if (projects != null)
            {
                return new ObjectResult(projects);
            }

            return StatusCode(404);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetProject(int id)
        {
            var project = await _gitLabHandler.GetProject(id);

            if (project != null)
            {
                return new ObjectResult(project);
            }

            return StatusCode(404);
        }

        [HttpGet]
        [Route("{id}/{commitId}")]
        public async Task<IActionResult> GetProjectCommitDetails(int id, Guid commitId)
        {
            var commit = await _gitLabHandler.GetProjectCommit(id, commitId);

            if (commit != null)
            {
                return new ObjectResult(commit);
            }

            return StatusCode(404);
        }

        [HttpGet]
        [Route("{id}/file-history")]
        public async Task<IActionResult> GetFileHistory()
        {
            return StatusCode(200);
        }
    }
}