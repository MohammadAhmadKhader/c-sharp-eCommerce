using System.ComponentModel.DataAnnotations.Schema;
using c_sharp_eCommerce.Core.Models.Contracts;

namespace c_sharp_eCommerce.Core.Models
{
    public class Order: IAuditable
    {
        public int Id { get; set; }

        [ForeignKey(nameof(User))]
        public Guid UserId { get; set; }
        public string Status { get; set; } = "pending";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public User? User { get; set; }
        public ICollection<OrderDetails> OrderDetails { get; set; } = new HashSet<OrderDetails>();
    }
}
