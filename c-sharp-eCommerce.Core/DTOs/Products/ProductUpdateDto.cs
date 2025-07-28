using Microsoft.AspNetCore.Http;

namespace c_sharp_eCommerce.Core.DTOs.Products
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
