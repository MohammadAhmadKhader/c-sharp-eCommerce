using c_sharp_eCommerce.Core.DTOs.Orders;
using FluentValidation;

namespace c_sharp_eCommerce.API.Validations.OrderValidations
{
    public class OrderCreateDtoValidator : AbstractValidator<OrderCreateDto>
    {
        public OrderCreateDtoValidator()
        {
            RuleFor(x => x.Items)
                .Must(x => x.Count > 0).WithMessage("order items are required");
            RuleForEach(x => x.Items)
                .SetValidator(new OrderItemDtoValidator());

            RuleFor(x => x)
                .Must(x => Guid.TryParse(x.UserId, out _)).WithMessage("User id is not a correct Guid string");
        }
    }
}
