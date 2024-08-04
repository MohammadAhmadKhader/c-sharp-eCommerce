using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace c_shap_eCommerce.Core.Models
{
    public class Order
    {
        public int Id { get; set; }

        [ForeignKey(nameof(User))]
        public Guid UserId { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public User? User { get; set; }
        public ICollection<OrderDetails> OrderDetails { get; set; } = new HashSet<OrderDetails>();
    }
}
