using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace c_shap_eCommerce.Core.DTOs.Products
{
	public class ProductUpdateDto
    {
		public string? Name { get; set; }
		public string? Description { get; set; }
        public IFormFile? Image { get; set; }
		public double? Price { get; set; }
		public int? Quantity { get; set; }
		public int? CategoryId { get; set; }
    }
}
