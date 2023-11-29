using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tourism_Guidance_And_Networking.Core.Consts;

namespace Tourism_Guidance_And_Networking.Web.Controllers
{
	[Authorize(Roles =Roles.Admin)]
	[Route("api/[controller]")]
	[ApiController]
	public class SecuredController : ControllerBase
	{
		[HttpGet]
		public IActionResult Get()
		{
			return Ok("Accessed Sucessfully");
		}
	}
}
