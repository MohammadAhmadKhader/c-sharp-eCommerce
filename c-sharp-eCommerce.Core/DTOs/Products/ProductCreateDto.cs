using Microsoft.AspNetCore.Http;

namespace c_sharp_eCommerce.Core.DTOs.Products
{
	public class ProductCreateDto
	{
		public string Name { get; set; } = null!;
		public string Description { get; set; } = null!;
		public IFormFile Image { get; set; } = null!;
		public double? Price { get; set; }
		public int? Quantity { get; set; }
		public int? CategoryId { get; set; }
	}
}
