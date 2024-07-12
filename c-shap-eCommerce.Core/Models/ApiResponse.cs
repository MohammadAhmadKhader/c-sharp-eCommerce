using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace c_shap_eCommerce.Core.Models
{
    public class ApiResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public bool IsSucess { get; set; }
        public string ErrorMessages { get; set; }
        public object Result { get; set; } // object => any in TypeScript
    }
}
