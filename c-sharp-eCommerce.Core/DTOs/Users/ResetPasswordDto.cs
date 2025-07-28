namespace c_sharp_eCommerce.Core.DTOs.Users
{
	public class ResetPasswordDto
	{
		public string Email { get; set; } = null!;
		public string NewPassword { get; set; } = null!;
		public string ConfirmPassword { get; set; } = null!;
		public string Token { get; set; } = null!;
	}
}
