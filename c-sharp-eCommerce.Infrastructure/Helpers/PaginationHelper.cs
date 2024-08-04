using c_shap_eCommerce.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace c_sharp_eCommerce.Infrastructure.Helpers
{
	public static class PaginationHelper
	{
		public const int LimitUserMin = 4;
		public const int LimitUserMax = 30;
		public const int LimitAdminMin = 4;
		public const int LimitAdminMax = 60;
		public const int DefaultLimit = 9;
		public const int DefaultPage = 1;
		public static int ValidatePage(int? page)
		{
			if (page == null || page < 1)
			{
				return DefaultPage;
			}
			return (int)page;
		}
		public static int ValidateLimit(int? limit, User? user = null)
		{
			int expectedMaxLimit = LimitUserMax;
			int expectedMinLimit = LimitUserMin;
			// must validate if user is not admin
			if (user is not null)
			{
				expectedMaxLimit = LimitAdminMax;
				expectedMinLimit = LimitAdminMin;
			}
			if (limit == null || limit < expectedMinLimit)
			{
				return DefaultLimit;
			}
			if(limit > expectedMaxLimit)
			{
				return expectedMaxLimit;
			}

            return (int)limit;
		}
	}
}
