using c_shap_eCommerce.Core.DTOs.OrderDetails;
using c_shap_eCommerce.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace c_shap_eCommerce.Core.DTOs.Orders
{
	public class OrderDto
	{
		public int Id { get; set; }
		public Guid UserId { get; set; }
		public string Status { get; set; }
		public DateTime CreatedAt { get; set; } = DateTime.Now;
		public DateTime UpdatedAt { get; set; } = DateTime.Now;
		public List<OrderDetailsDto>? OrderDetails { get; set; }
	}
}
