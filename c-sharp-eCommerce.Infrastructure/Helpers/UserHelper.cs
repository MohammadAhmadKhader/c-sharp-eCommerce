using Microsoft.AspNetCore.Identity;

namespace c_sharp_eCommerce.Infrastructure.Helpers
{
	public static class UserHelper
	{
		public static string CaptureManagerError(IEnumerable<IdentityError> errs)
		{
			var errorsAsString = string.Join(", ", errs.Select(x => x.Description));
			return errorsAsString;
		}
	}
}
