using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tourism_Guidance_And_Networking.Core.Consts;
using Tourism_Guidance_And_Networking.Core.Models.Authentication;
using Tourism_Guidance_And_Networking.Web.Services;

namespace Tourism_Guidance_And_Networking.Web.Controllers
{
    [EnableCors("AllowAnyOrigin")]
    [Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
	   private readonly IAuthService _authService;

		public AuthController(IAuthService authService)
		{
			_authService = authService;
		}

		[HttpPost("register")]
		public async Task<IActionResult> RegisterAsync([FromBody] RegisterModel model)
		{
			if(!ModelState.IsValid)
				return BadRequest(ModelState);

			var result = await _authService.RegisterAsync(model);

			if(!result.IsAuthenticated)
				return BadRequest(result.Message);

			SetRefreshTokenInCookies(result.RefreshToken, result.RefrshTokenExpiration);

			return Ok(result);

		}

		[HttpPost("login")]
		public async Task<IActionResult> LoginAsync([FromBody] LoginModel model)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var result = await _authService.LoginAsync(model);

			if (!result.IsAuthenticated)
				return BadRequest(result.Message);

			if (!string.IsNullOrEmpty(result.RefreshToken))
				SetRefreshTokenInCookies(result.RefreshToken, result.RefrshTokenExpiration);

			return Ok(result);

		}
		[Authorize(Roles =Roles.Admin)]
		[HttpPost("addRole")]
		public async Task<IActionResult> AddRoleAsync([FromBody] AddRoleModel model)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var result = await _authService.AddRoleAsync(model);

			if (!string.IsNullOrEmpty(result))
				return BadRequest(result);

			return Ok("User Assigned Successfullly");
		}

		[HttpGet("refreshToken")]
		public async Task<IActionResult> RefreshToken()
		{
			var refreshToken = Request.Cookies["refreshToken"];

			var result = await _authService.RefreshTokenAsync(refreshToken);

			if (!result.IsAuthenticated)
				return BadRequest(result);

			SetRefreshTokenInCookies(result.RefreshToken, result.RefrshTokenExpiration);
			return Ok(result);
		}
		[HttpGet("revokeToken")]
		public async Task<IActionResult> RevokeToken()
		{
			var refreshToken = Request.Cookies["refreshToken"];

			if (string.IsNullOrEmpty(refreshToken))
				return BadRequest("Token is required");

			var result = await _authService.RevokeTokenAsync(refreshToken);

			if (!result)
				return BadRequest("Invalid Token");

			return Ok("Revoked Successfully");
		}
		private void SetRefreshTokenInCookies(string refreshToken, DateTime expires)
		{
			var cookieOptions = new CookieOptions()
			{
				HttpOnly = true,
				Expires = expires.ToLocalTime(), // important as time in postman diffrent from local
				Secure = true,
				IsEssential = true,
				SameSite = SameSiteMode.None

			};

			Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
		}


	}
}
