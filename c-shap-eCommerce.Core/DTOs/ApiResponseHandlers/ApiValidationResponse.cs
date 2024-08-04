using c_shap_eCommerce.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace c_shap_eCommerce.Core.DTOs.ApiResponseHandlers
{
	public class ApiValidationResponse : ApiResponse
	{
		public IEnumerable<string> Errors { get; set; }
		public ApiValidationResponse(HttpStatusCode statusCode, IEnumerable<string>? errors = null, string? message=null)
			:base(statusCode, message)
		{
			Errors = errors ?? new List<string>();
		}
		
	}
}
