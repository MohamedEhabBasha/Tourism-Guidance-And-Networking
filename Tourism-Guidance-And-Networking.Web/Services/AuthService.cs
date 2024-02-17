using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Tourism_Guidance_And_Networking.Core.Consts;
using Tourism_Guidance_And_Networking.Core.Helper;
using Tourism_Guidance_And_Networking.Core.Models;
using Tourism_Guidance_And_Networking.Core.Models.Authentication;

namespace Tourism_Guidance_And_Networking.Web.Services
{
	public class AuthService : IAuthService
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly JWT _jwt;
		public AuthService(UserManager<ApplicationUser> userManager, IOptions<JWT> jwt, RoleManager<IdentityRole> roleManager)
		{
			_userManager = userManager;
			_jwt = jwt.Value;
			_roleManager = roleManager;
		}

		public async Task<string> AddRoleAsync(AddRoleModel model)
		{
			var user = await _userManager.FindByNameAsync(model.UserName);

			if (user is null || !await _roleManager.RoleExistsAsync(model.Role))
				return "Invalid username or Role";

			if (await _userManager.IsInRoleAsync(user, model.Role))
				return "User already assigned to this role";

			var result = await _userManager.AddToRoleAsync(user, model.Role);

			return result.Succeeded ? string.Empty : "Sonething went wrong";

		}

		public async Task<AuthModel> LoginAsync(LoginModel model)
		{
			var authModel = new AuthModel();	
			var user = await  _userManager.FindByEmailAsync(model.Email);

			if(user is null || !await _userManager.CheckPasswordAsync(user,model.Password))
			{
				authModel.Message = "Email or Password is Incorrect!";
				return authModel;
			}

			var jwtSecurityToken = await CreateJwtToken(user);
			var roles = await _userManager.GetRolesAsync(user);

			authModel.Email = user.Email;
			authModel.Username = user.UserName;
			authModel.IsAuthenticated  = true;
			//authModel.ExpiresOn = jwtSecurityToken.ValidTo;
			authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
			authModel.Roles = roles.ToList();
			authModel.Message = "Successfully Login";

			if (user.RefreshTokens.Any(r => r.IsActive))
			{
				var activeRefreshToken = user.RefreshTokens.FirstOrDefault(r => r.IsActive);
				authModel.RefreshToken = activeRefreshToken.Token;
				authModel.RefrshTokenExpiration = activeRefreshToken.ExpiresOn;
			}
			else
			{
				var refreshToken = GenerateRefreshToken();
				authModel.RefreshToken = refreshToken.Token;
				authModel.RefrshTokenExpiration = refreshToken.ExpiresOn;
				user.RefreshTokens.Add(refreshToken);
				await _userManager.UpdateAsync(user);
			}


			return authModel;

		}

		public async Task<AuthModel> RegisterAsync(RegisterModel model)
		{
			if (await _userManager.FindByEmailAsync(model.Email) is not null)
				return new AuthModel { Message = "Email is already registered !" };
			if (await _userManager.FindByNameAsync(model.Username) is not null)
				return new AuthModel { Message = "Username is already registered !" };

			var user = new ApplicationUser { 
				Email = model.Email,
				FirstName = model.FirstName,
				LastName = model.LastName,
				UserName=model.Username,
			};

			var result = await _userManager.CreateAsync(user,model.Password);
			if (!result.Succeeded)
			{
				var errors = string.Empty;
				foreach (var error in result.Errors)
				{
					errors += $"{error.Description} \n"; 
				}
				return new AuthModel { Message = errors };
			}

            //create roles if they are not created
            if (!_roleManager.RoleExistsAsync(Roles.Admin).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(Roles.Admin)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(Roles.Tourist)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(Roles.Company)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(Roles.Hotel)).GetAwaiter().GetResult();

				ApplicationUser userAdmin = new ApplicationUser()
				{
					UserName = "admin@gmail.com",
					Email = "admin@gmail.com",
					FirstName = "Anton",
					LastName = "Samoel",
					PhoneNumber = "1112223333",
				};
				
                await _userManager.CreateAsync(userAdmin,"Admin@2001");
                _userManager.AddToRoleAsync(userAdmin, Roles.Admin).GetAwaiter().GetResult();

            }

            await _userManager.AddToRoleAsync(user, Roles.Tourist); 
			
			var jwtSecurityToken = await CreateJwtToken(user);
            var refreshToken = GenerateRefreshToken();
            return new AuthModel
			{
				Email=user.Email,
				Username = user.UserName,
				IsAuthenticated = true,
				Message = "Successfully Registered",
				Roles = new List<string> { Roles.Tourist },
				Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
				RefreshToken = refreshToken.Token,
				RefrshTokenExpiration = refreshToken.ExpiresOn,
			};

		}

		private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
		{
			var userClaims = await _userManager.GetClaimsAsync(user);
			var roles = await _userManager.GetRolesAsync(user);
			var roleClaims = new List<Claim>();

			foreach (var role in roles)
				roleClaims.Add(new Claim("roles", role));

			var claims = new[]
			{
				new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
				new Claim(JwtRegisteredClaimNames.Email, user.Email),
				new Claim("userId", user.Id)
			}
			.Union(userClaims)
			.Union(roleClaims);

			var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
			var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

			var jwtSecurityToken = new JwtSecurityToken(
				issuer: _jwt.Issuer,
				audience: _jwt.Audience,
				claims: claims,
				expires: DateTime.Now.AddMinutes(_jwt.DurationInMinutes),
				signingCredentials: signingCredentials);

			return jwtSecurityToken;
		}
		public RefreshToken GenerateRefreshToken()
		{
			var randomNumber = new byte[32];
			using var generator = new RNGCryptoServiceProvider();

			generator.GetBytes(randomNumber);

			return new RefreshToken()
			{
				Token = Convert.ToBase64String(randomNumber),
				ExpiresOn = DateTime.UtcNow.AddHours(5),
				CreatedOn = DateTime.UtcNow,
			};
		}

		public async Task<AuthModel> RefreshTokenAsync(string token)
		{
			var authModel = new AuthModel();

			var user = await _userManager.Users.SingleOrDefaultAsync(u => u.RefreshTokens.Any(r => r.Token == token));

			if (user is null)
			{
				// Is Authenticated is false by default
				authModel.Message = "Invalid Token";
				return authModel;
			}

			var refreshToken = user.RefreshTokens.Single(r => r.Token == token);

			if (!refreshToken.IsActive)
			{
				// Is Authenticated is false by default
				authModel.Message = "Inactive Token";
				return authModel;
			}

			refreshToken.RevokedOn = DateTime.UtcNow;

			var newRefreshToken = GenerateRefreshToken();
			user.RefreshTokens.Add(newRefreshToken);
			user.RefreshTokens.Remove(refreshToken);
			await _userManager.UpdateAsync(user);

			var jwtToken = await CreateJwtToken(user);

			authModel.IsAuthenticated = true;
			authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
			authModel.Email = user.Email;
			authModel.Username = user.UserName;
			var roles = await _userManager.GetRolesAsync(user);
			authModel.Roles = roles.ToList();

			authModel.RefreshToken = newRefreshToken.Token;
			authModel.RefrshTokenExpiration = newRefreshToken.ExpiresOn;


			return authModel;
		}

		public async Task<bool> RevokeTokenAsync(string token)
		{
			var user = await _userManager.Users.SingleOrDefaultAsync(u => u.RefreshTokens.Any(r => r.Token == token));

			if (user is null)
				return false;

			var refreshToken = user.RefreshTokens.Single(r => r.Token == token);

			if (!refreshToken.IsActive)
				return false;


			refreshToken.RevokedOn = DateTime.UtcNow;
			user.RefreshTokens.Remove(refreshToken);
			await _userManager.UpdateAsync(user);

			return true;
		}
	}
}
