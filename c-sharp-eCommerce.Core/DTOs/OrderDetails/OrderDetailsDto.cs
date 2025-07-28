namespace c_sharp_eCommerce.Core.DTOs.OrderDetails
{
	public class OrderDetailsDto
	{
		public int ProductId { get; set; }
		public int OrderId { get; set; }
		public int Quantity { get; set; }
		public double Price { get; set; }
	}
}
