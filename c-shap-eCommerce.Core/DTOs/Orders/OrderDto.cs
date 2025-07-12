using c_shap_eCommerce.Core.DTOs.OrderDetails;

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