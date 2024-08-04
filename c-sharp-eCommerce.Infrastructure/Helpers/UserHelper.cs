using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
