using c_sharp_eCommerce.Core.DTOs.Tokens;
using c_sharp_eCommerce.Core.IServices;
using c_sharp_eCommerce.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace c_sharp_eCommerce.Services
{
	public class TokenService : ITokenService
	{
		private readonly IConfiguration _configuration;
		private readonly string _secretKey;
		private readonly UserManager<User> _userManager;
		private readonly IHttpContextAccessor _httpContextAccessor;
		public TokenService(IConfiguration configuration, UserManager<User> userManager, IHttpContextAccessor httpContextAccessor)
		{
			_configuration = configuration;
			_secretKey = configuration.GetSection("ApiSettings")["JWTSecretKey"];
			_userManager = userManager;
			_httpContextAccessor = httpContextAccessor;
		}

		public async Task<string> CreateTokenAsync(User user)
		{
			var key = Encoding.ASCII.GetBytes(_secretKey);

			var roles = await _userManager.GetRolesAsync(user);
			var claims = new List<Claim>
			{
				new Claim(ClaimTypes.Name, user.UserName),
				new Claim(ClaimTypes.Email, user.Email),
				new Claim("UserId", user.Id.ToString()),
			};

			claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(claims),
				Expires = DateTime.UtcNow.AddDays(7),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
			};

			var tokenHandler = new JwtSecurityTokenHandler();
			var token = tokenHandler.CreateToken(tokenDescriptor);
			var tokenAsString = tokenHandler.WriteToken(token);
			return tokenAsString;
		}

		public TokenData GetTokenData()
		{
			var userEmail = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value;
			var userName = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			var userId = _httpContextAccessor.HttpContext?.User.FindFirst("userId")?.Value;
			Console.WriteLine(userId);

			return new TokenData { UserId = userId!, Email = userEmail!, UserName = userName! };
		}
	}
}
