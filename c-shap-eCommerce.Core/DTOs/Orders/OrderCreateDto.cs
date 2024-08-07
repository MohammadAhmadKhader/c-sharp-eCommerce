using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace c_shap_eCommerce.Core.DTOs.Orders
{
	public class OrderCreateDto
	{
		[Required]
		public List<OrderItemDto> Items { get; set; }

		[Required]
		public string UserId { get; set; }
	}
}
