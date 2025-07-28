using c_sharp_eCommerce.Core.DTOs.Orders;
using FluentValidation;

namespace c_sharp_eCommerce.API.Validations.OrderValidations
{
	public class OrderItemDtoValidator : AbstractValidator<OrderItemDto>
	{
		public OrderItemDtoValidator()
		{
			RuleFor(x => x.Quantity)
				.NotEmpty().WithMessage("quantity is required")
				.InclusiveBetween(1, 100000).WithMessage("quantity must be between 1 and 100000");

			RuleFor(x => x.ProductId)
				.NotEmpty().WithMessage("product id is required")
				.InclusiveBetween(1, int.MaxValue).WithMessage($"quantity must be between 1 and {int.MaxValue}");
		}
	}
}
