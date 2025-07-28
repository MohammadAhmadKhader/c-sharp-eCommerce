using c_sharp_eCommerce.Core.Models;
using c_sharp_eCommerce.Core.DTOs.Tokens;

namespace c_sharp_eCommerce.Core.IServices
{
    public interface ITokenService
    {
        Task<string> CreateTokenAsync(User user);
        TokenData GetTokenData();
    }
}
