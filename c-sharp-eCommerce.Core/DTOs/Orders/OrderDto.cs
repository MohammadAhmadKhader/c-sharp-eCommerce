using c_sharp_eCommerce.Core.DTOs.OrderDetails;

namespace c_sharp_eCommerce.Core.DTOs.Orders
{
	public class OrderDto
	{
		public int Id { get; set; }
		public Guid UserId { get; set; }
		public string Status { get; set; } = null!;
		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
		public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
		public List<OrderDetailsDto>? OrderDetails { get; set; }
	}
}
