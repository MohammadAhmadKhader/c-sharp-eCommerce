namespace c_sharp_eCommerce.Core.DTOs.Users
{
	public class LoginResponseDto
	{
		public string Token { get; set; } = null!;
		public UserDto User { get; set; } = null!;
		public string Role { get; set; } = null!;
	}
}
