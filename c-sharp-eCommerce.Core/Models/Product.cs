using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using c_sharp_eCommerce.Core.Models.Contracts;

namespace c_sharp_eCommerce.Core.Models
{
    public class Product: IAuditable
    {
        public int Id { get; set; }

        [MinLength(3)]
        [MaxLength(64)]
        public string Name { get; set; } = null!;

		[MinLength(3)]
		[MaxLength(1024)]
		public string Description { get; set; } = null!;
        public string Image { get; set; } = null!;

		[Range(0.0, 1000000, ErrorMessage = "minimum quantity allowed is 0 and max 1,000,000")]
		public double Price { get; set; }

		[Range(0, 1000000, ErrorMessage = "minimum quantity allowed is 0 and max 1,000,000")]
		public int Quantity { get; set; }

        [ForeignKey(nameof(Category))]
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
        public ICollection<OrderDetails> OrderDetails { get; set; } = new HashSet<OrderDetails>();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

	}
}
