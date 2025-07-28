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
		[Obsolete]
		private static readonly Dictionary<string, bool> paginatedRoutes = new()
		{
			{"/api/products",true},
			{"/api/products/categories",true},
			{"/api/categories",true},

		};
		public static int ValidatePage(int? page)
		{
			if (page == null || page < 1)
			{
				return DefaultPage;
			}
			return (int)page;
		}
		public static int ValidateLimit(int? limit, bool isAdmin = false)
		{
			int expectedMaxLimit = LimitUserMax;
			int expectedMinLimit = LimitUserMin;
			// must validate if user is not admin
			if (isAdmin)
			{
				expectedMaxLimit = LimitAdminMax;
				expectedMinLimit = LimitAdminMin;
			}
			if (limit == null || limit < expectedMinLimit)
			{
				return DefaultLimit;
			}
			if (limit > expectedMaxLimit)
			{
				return expectedMaxLimit;
			}

			return (int)limit;
		}
		[Obsolete]
		public static bool IsPaginatedPath(string path)
		{
			return paginatedRoutes.ContainsKey(path) == true;
		}
		public static (int, int) ValidatePageAndLimit(int? page, int? limit, bool isAdmin = false)
		{
			var validatedpage = ValidatePage(page);
			var validatedLimit = ValidateLimit(limit, isAdmin);
			return (validatedpage, validatedLimit);
		}

	}
}
