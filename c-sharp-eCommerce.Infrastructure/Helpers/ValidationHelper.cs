using Microsoft.AspNetCore.Mvc;

namespace c_sharp_eCommerce.Infrastructure.Helpers
{
	public class ValidationHelper
	{
		public static List<string> GetValidationErrors(ActionContext actionContext)
		{
			var errors = actionContext.ModelState
				.Values
				.SelectMany(x => x.Errors)
				.Select(err => err.ErrorMessage)
				.ToList();

			return errors;
		}
	}
}
