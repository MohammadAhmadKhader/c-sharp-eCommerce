using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace c_sharp_eCommerce.Core.Models
{
    public class OrderDetails
    {
        public int Id { get; set; }

        [ForeignKey(nameof(Order))]
        public int OrderId { get; set; }

        [ForeignKey(nameof(Product))]
        public int ProductId { get; set; }

		[Range(0.0, 1000000, ErrorMessage = "minimum quantity allowed is 0 and max 1,000,000")]
		public double Price { get; set; }

		[Range(0, 1000000, ErrorMessage = "minimum quantity allowed is 0 and max 1,000,000")]
		public int Quantity { get; set; }
        public Order? Order { get; set; }
        public Product? Product { get; set; }
    }
}
