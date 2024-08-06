using c_shap_eCommerce.Core.DTOs.OrderDetails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace c_shap_eCommerce.Core.DTOs.Orders
{
	public class OrderResponseDto
	{
		public int OrderId { get; set; }
		public DateTime CreatedAt { get; set; }
		public double TotalPrice { get; set; }
		public List<OrderDetailsDto> OrderDetails { get; set; }
	}
}
