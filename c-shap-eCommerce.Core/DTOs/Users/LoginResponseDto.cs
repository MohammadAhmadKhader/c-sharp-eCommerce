﻿using c_shap_eCommerce.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace c_shap_eCommerce.Core.DTOs.Users
{
	public class LoginResponseDto
	{
		public string Token { get; set; }
		public UserDto User { get; set; }
		public string Role { get; set; }
	}
}
