namespace c_sharp_eCommerce.Core.DTOs.Users
{
	public class UserDto
	{
		public Guid Id { get; set; }
		public string FirstName { get; set; } = null!;
		public string LastName { get; set; } = null!;
		public string UserName { get; set; } = null!;
		public string Email { get; set; } = null!;
		public string? Address { get; set; }
		public bool EmailConfirmed { get; set; }
		public string? PhoneNumber { get; set; }
		public bool PhoneNumberConfirmed { get; set; }
	}
}
