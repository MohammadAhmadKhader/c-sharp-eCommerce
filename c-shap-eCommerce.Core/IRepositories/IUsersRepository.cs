using c_shap_eCommerce.Core.DTOs.Users;
using c_shap_eCommerce.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace c_shap_eCommerce.Core.IRepositories
{
	public interface IUsersRepository
	{
		public bool IsUniqueUser(string Email);
		public Task<LoginResponseDto> Login(LoginRequestDto loginRequest);
		public Task<UserDto> Register(RegisterationRequestDto registerationRequest);

		
	}
}
