using c_sharp_eCommerce.Core.DTOs.Products;
using c_sharp_eCommerce.API.Validations.ValidatorsExtensions;
using FluentValidation;
using FluentValidation.Results;
using static c_sharp_eCommerce.API.Validations.ProductValidations.ProductValidators;

namespace c_sharp_eCommerce.API.Validations.ProductValidations
{
    public class ProductUpdateDtoValidator : AbstractValidator<ProductUpdateDto>
    {
        public ProductUpdateDtoValidator()
        {
            RuleFor(x => x.CategoryId)
                .SetValidator(new ProductCategoryIdValidator());

            RuleFor(x => x.Name)
                .SetValidator(new ProductNameValidator());

            RuleFor(x => x.Description)
                .SetValidator(new ProductDescriptionValidator());

            RuleFor(x => x.Image)
                .CustomImageValidator();

            RuleFor(x => x.Price)
                .SetValidator(new ProductPriceValidator());

            RuleFor(x => x.Quantity)
                .SetValidator(new ProductQuantityValidator());

            RuleFor(x => x).Custom((product, context) =>
            {
                if (
                    string.IsNullOrWhiteSpace(product.Description) &&
                    (product.Image == null || product.Image.Length == 0) &&
                    string.IsNullOrWhiteSpace(product.Name) &&
                    !product.Quantity.HasValue &&
                    !product.Price.HasValue &&
                    !product.CategoryId.HasValue
                )
                {
                    context.AddFailure("At least one of the fields is required (name, description, image, quantity, price, categoryId)");
                }
            });
        }
        // Trimming Strings
        protected override bool PreValidate(ValidationContext<ProductUpdateDto> context, ValidationResult result)
        {
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
