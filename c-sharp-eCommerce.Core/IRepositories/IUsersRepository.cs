using c_sharp_eCommerce.Core.DTOs.Users;

namespace c_sharp_eCommerce.Core.IRepositories
{
	public interface IUsersRepository
	{
		public bool IsUniqueUser(string email);
		public Task<LoginResponseDto> Login(LoginRequestDto loginRequest);
		public Task<UserDto> Register(RegisterationRequestDto registerationRequest);
	}
}
