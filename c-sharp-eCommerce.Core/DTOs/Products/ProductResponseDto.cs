namespace c_sharp_eCommerce.Core.DTOs.Products
{
	public class ProductResponseDto
	{
		public int Id { get; set; }
		public string Name { get; set; } = null!;
		public string Description { get; set; } = null!;
		public string Image { get; set; } = null!;
		public double Price { get; set; }
		public int Quantity { get; set; }
		public string? Category { get; set; }
	}
}
