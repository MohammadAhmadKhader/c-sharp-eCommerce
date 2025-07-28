using System.Net;
using System.Text.Json.Serialization;

namespace c_sharp_eCommerce.Core.DTOs.ApiResponse
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
			IsSucess = (int)statusCode >= 200 && (int)statusCode < 300 ? true : false;
			Data = data;
		}
		public ApiResponse(HttpStatusCode statusCode, string? message)
		{
			StatusCode = statusCode;
			Message = message;
			IsSucess = false;
		}

		public ApiResponse(HttpStatusCode statusCode, bool isSuccess, string message)
		{
			StatusCode = statusCode;
			Message = message;
			IsSucess = isSuccess;
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
