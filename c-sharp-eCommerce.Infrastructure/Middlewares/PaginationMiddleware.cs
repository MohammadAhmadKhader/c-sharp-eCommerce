using c_sharp_eCommerce.Infrastructure.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace c_sharp_eCommerce.Infrastructure.Middlewares
{
    [Obsolete]
	public class PaginationMiddleware
	{
        private readonly RequestDelegate next;
        public PaginationMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {

            var path = context.Request.Path.ToString();
			if (PaginationHelper.IsPaginatedPath(path.ToLower()))
            {
			    var query = context.Request.Query;
                var page = PaginationHelper.DefaultPage;
			    var limit = PaginationHelper.DefaultLimit;
			    if (query.ContainsKey("page"))
                {
                    if (int.TryParse(query["page"], out int parsedPage))
                    {
                        page = PaginationHelper.ValidatePage(parsedPage);
                    }
                }
			    if (query.ContainsKey("limit"))
			    {
			    	if (int.TryParse(query["limit"], out int parsedLimit))
			    	{
			    		limit = PaginationHelper.ValidateLimit(parsedLimit);
			    	}
			    }

                var modifiedQuery = new QueryCollection(query.ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Key == "page" ? new StringValues(page.ToString()) : (kvp.Key == "limit" ? new StringValues(limit.ToString()) : kvp.Value)
			    ));

                context.Request.Query = modifiedQuery;
            }
            await next(context);
		}
    }
}
