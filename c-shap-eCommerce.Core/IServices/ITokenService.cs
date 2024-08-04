using c_shap_eCommerce.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace c_shap_eCommerce.Core.IServices
{
    public interface ITokenService
    {
        Task<string> CreateTokenAsync(User user);
    }
}
