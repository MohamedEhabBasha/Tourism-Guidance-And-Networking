using Tourism_Guidance_And_Networking.Core.Models.Authentication;

namespace Tourism_Guidance_And_Networking.Web.Services
{
	public interface IAuthService
	{
		Task<AuthModel> RegisterAsync(RegisterModel model);
		Task<AuthModel> LoginAsync(LoginModel model);
		Task<string> AddRoleAsync(AddRoleModel model);
		public Task<AuthModel> RefreshTokenAsync(string token);
		public Task<bool> RevokeTokenAsync(string token);

	}
}
