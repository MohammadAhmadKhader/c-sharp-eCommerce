using System.ComponentModel.DataAnnotations;

namespace c_sharp_eCommerce.Core.DTOs.Orders
{
	public class OrderCreateDto
	{
		[Required]
		public List<OrderItemDto> Items { get; set; } = default!;

		[Required]
		public string UserId { get; set; } = null!;
	}
}
