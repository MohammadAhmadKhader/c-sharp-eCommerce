namespace c_shap_eCommerce.Core.DTOs.Users
{
    public class LoginResponseDto
    {
        public string Token { get; set; }
        public UserDto User { get; set; }
        public string Role { get; set; }
    }
}