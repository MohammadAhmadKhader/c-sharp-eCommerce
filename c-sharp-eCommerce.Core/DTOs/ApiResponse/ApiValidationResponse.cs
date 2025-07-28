using System.Net;

namespace c_sharp_eCommerce.Core.DTOs.ApiResponse
{
	public class ApiValidationResponse : ApiResponse
	{
		public IEnumerable<string> Errors { get; set; }
		public ApiValidationResponse(HttpStatusCode statusCode, IEnumerable<string>? errors = null, string? message = null)
			: base(statusCode, message)
		{
			Errors = errors ?? new List<string>();
		}

	}
}
