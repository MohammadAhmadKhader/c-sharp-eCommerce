using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace c_shap_eCommerce.Core.DTOs.Users
{
	public class LoginRequestDto
	{
		public string Email { get; set; }
		public string Password { get; set; }
	}
}
