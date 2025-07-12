using System.ComponentModel.DataAnnotations;

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