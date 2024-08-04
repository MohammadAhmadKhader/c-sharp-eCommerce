using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace c_shap_eCommerce.Core.Models
{
    public class Product
    {
        public int Id { get; set; }

        [MinLength(3)]
        [MaxLength(64)]
        public string Name { get; set; }

		[MinLength(3)]
		[MaxLength(1024)]
		public string Description { get; set; }
        public string Image { get; set; }

		[Range(0.0, 1000000, ErrorMessage = "minimum quantity allowed is 0 and max 1,000,000")]
		public double Price { get; set; }

		[Range(0, 1000000, ErrorMessage = "minimum quantity allowed is 0 and max 1,000,000")]
		public int Quantity { get; set; }

        [ForeignKey(nameof(Category))]
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
        public ICollection<OrderDetails> OrderDetails { get; set; } = new HashSet<OrderDetails>();

    }
}
