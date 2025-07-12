using c_shap_eCommerce.Core.Models;

namespace c_shap_eCommerce.Core.IServices
{
    public interface ITokenService
    {
        Task<string> CreateTokenAsync(User user);
        TokenData GetTokenData();
    }
}