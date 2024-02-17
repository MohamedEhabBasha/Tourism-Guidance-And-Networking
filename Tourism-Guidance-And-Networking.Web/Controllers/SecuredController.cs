﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tourism_Guidance_And_Networking.Core.Consts;

namespace Tourism_Guidance_And_Networking.Web.Controllers
{
	
	[Route("api/[controller]")]
	[ApiController]
	public class SecuredController : ControllerBase
	{
		[Authorize(Roles = Roles.Admin)]
		[HttpGet]
		public IActionResult Get()
		{
			return Ok("Accessed Sucessfully");
		}

		[Authorize]
		[HttpGet("data")]
		public IActionResult GetData()
		{
			return Ok("Data Returned");
		}
	}
}
