using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace c_shap_eCommerce.Core.DTOs.Products
{
	public class ProductCreateDto
	{
		[MinLength(2)]
		[MaxLength(32)]
		public string Name { get; set; }

		[MinLength(6)]
		[MaxLength(256)]
		public string Description { get; set; }

		[MinLength(6)]
		[MaxLength(128)]
		public string Image { get; set; }

		[Range(0.0, 10000, ErrorMessage = "{0} has exceeded the limitation.")]
		[Required(ErrorMessage = "price is required")]
		public double? Price { get; set; }

		[Range(0, 1000000, ErrorMessage = "minimum quantity allowed is 0 and max 1,000,000")]
		public int? Quantity { get; set; }

		[Range(0,int.MaxValue)]
		[Required(ErrorMessage = "categoryId is required")]
		public int? CategoryId { get; set; }
	}
}
