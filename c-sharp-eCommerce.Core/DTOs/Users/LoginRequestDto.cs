namespace c_sharp_eCommerce.Core.DTOs.Users
{
	public class LoginRequestDto
	{
		public string Email { get; set; } = null!;
		public string Password { get; set; } = null!;
	}
}
