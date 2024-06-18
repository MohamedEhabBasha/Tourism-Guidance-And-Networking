using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Tourism_Guidance_And_Networking.Core.Consts;
using Tourism_Guidance_And_Networking.Web.Services.AI;

namespace Tourism_Guidance_And_Networking.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AIInegrationController : ControllerBase
    {
        private readonly IExternalService _externalService;
        public AIInegrationController(IExternalService externalService)
        {
            _externalService = externalService;
        }

        [HttpPost("SeintmentAnalysis")]
        public async Task<IActionResult> MakeSeintmentAnalysis([FromBody] List<CommentsDTO> comments)
        {
            var response = await _externalService.PostDataToBackendAsync(comments);
            return Ok(response);
        }
    }
}
