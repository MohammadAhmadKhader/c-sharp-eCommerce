using c_shap_eCommerce.Core.IServices;
using c_shap_eCommerce.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace c_sharp_eCommerce.Services
{
	public class TokenService : ITokenService
	{
		private readonly IConfiguration configuration;
		private readonly string secretKey;
		private readonly UserManager<User> userManager;
		public TokenService(IConfiguration configuration, UserManager<User> userManager)
		{
			this.configuration = configuration;
			this.secretKey = configuration.GetSection("ApiSettings")["JWTSecretKey"];
			this.userManager = userManager;
		}

		public async Task<string> CreateTokenAsync(User user)
		{
			var key = Encoding.ASCII.GetBytes(secretKey);

			var roles = await userManager.GetRolesAsync(user);
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
	}
}
