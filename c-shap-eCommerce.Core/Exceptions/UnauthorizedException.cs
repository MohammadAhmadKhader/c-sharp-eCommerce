using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace c_shap_eCommerce.Core.Exceptions
{
	public class UnauthorizedException : Exception
	{
		public static int StatusCode = 401;
        public UnauthorizedException(string message) : base(message)
        {
            
        }
    }
}
