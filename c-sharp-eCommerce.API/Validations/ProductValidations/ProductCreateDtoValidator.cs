using c_sharp_eCommerce.Core.DTOs.Products;
using c_sharp_eCommerce.API.Validations.ValidatorsExtensions;
using FluentValidation;
using FluentValidation.Results;
using static c_sharp_eCommerce.API.Validations.ProductValidations.ProductValidators;

namespace c_sharp_eCommerce.API.Validations.ProductValidations
{
    public class ProductCreateDtoValidator : AbstractValidator<ProductCreateDto>
    {
        public ProductCreateDtoValidator()
        {
            RuleFor(x => x.CategoryId)
                .NotNull().WithMessage("category id can not be null")
                .SetValidator(new ProductCategoryIdValidator());

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("name is required")
                .SetValidator(new ProductNameValidator());

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("description is required")
                .SetValidator(new ProductDescriptionValidator());

            RuleFor(x => x.Image)
                .NotEmpty().WithMessage("image is required")
                .CustomImageValidator();

            RuleFor(x => x.Price)
                .NotEmpty().WithMessage("price is required")
                .SetValidator(new ProductPriceValidator());

            RuleFor(x => x.Quantity)
                .NotEmpty().WithMessage("quantity is required")
                .SetValidator(new ProductQuantityValidator());
        }
        // Trimming Strings
        protected override bool PreValidate(ValidationContext<ProductCreateDto> context, ValidationResult result)
        {
            Console.WriteLine($"Quantity: {context.InstanceToValidate.Quantity}");
            var instance = context.InstanceToValidate;
            if (instance != null)
            {
                if (instance.Name != null)
                {
                    context.InstanceToValidate.Name = instance.Name.Trim();
                }
                if (instance.Description != null)
                {
                    context.InstanceToValidate.Name = instance.Description.Trim();
                }
            }
            return base.PreValidate(context, result);
        }
    }
}
