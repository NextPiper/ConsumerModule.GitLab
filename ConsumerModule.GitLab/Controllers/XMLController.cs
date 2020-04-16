using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ConsumerModule.GitLab.Controllers
{
    [ApiController]
    [Route("data")]
    public class XMLController
    {
        [HttpGet]
        [Route("trial")]
        public async Task<IActionResult> GetTrial()
        {
            return new ObjectResult("trial");
        }
    }
}