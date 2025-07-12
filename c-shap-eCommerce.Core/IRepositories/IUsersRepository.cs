using c_shap_eCommerce.Core.DTOs.Users;

namespace c_shap_eCommerce.Core.IRepositories
{
    public interface IUsersRepository
    {
        public bool IsUniqueUser(string Email);
        public Task<LoginResponseDto> Login(LoginRequestDto loginRequest);
        public Task<UserDto> Register(RegisterationRequestDto registerationRequest);


    }
}