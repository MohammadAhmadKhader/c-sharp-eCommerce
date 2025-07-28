namespace c_sharp_eCommerce.Core.DTOs.Users
{
	public class RegisterationRequestDto
	{
		public string Email { get; set; } = null!;
		public string Password { get; set; } = null!;
		public string FirstName { get; set; } = null!;
		public string LastName { get; set; } = null!;

	}
}
