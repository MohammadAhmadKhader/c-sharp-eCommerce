using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace c_shap_eCommerce.Core.Models
{
    public class ApiResponse
    {
		public HttpStatusCode StatusCode { get; set; }
        public bool IsSucess { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Message { get; set; }

		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public object? Data { get; set; }
        public ApiResponse(HttpStatusCode statusCode, string message, object data)
        {
            StatusCode = statusCode;
			Message = message;
            IsSucess = (int) statusCode >= 200 && (int) statusCode < 300 ? true : false;
			Data = data;
        }
		public ApiResponse(HttpStatusCode statusCode, string? message)
		{
			StatusCode = statusCode;
			Message = message;
			IsSucess = false;
		}

		public ApiResponse(HttpStatusCode statusCode, object data)
		{
			StatusCode = statusCode;
			IsSucess = (int)statusCode >= 200 && (int)statusCode < 300 ? true : false;
			Data = data;
		}
		public static ApiResponse CreateBadRequest(string message)
        {
            return new ApiResponse(HttpStatusCode.BadRequest, message);
        }
    }
    
}
