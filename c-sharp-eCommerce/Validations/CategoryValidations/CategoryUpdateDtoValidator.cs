using c_shap_eCommerce.Core.DTOs.Categories;
using c_shap_eCommerce.Core.DTOs.Products;
using FluentValidation;
using FluentValidation.Results;
using static c_sharp_eCommerce.Validations.CategoryValidations.CategoryValidators;

namespace c_sharp_eCommerce.Validations.CategoryValidations
{
	public class CategoryUpdateDtoValidator : AbstractValidator<CategoryUpdateDto>
	{
        public CategoryUpdateDtoValidator()
        {
			RuleFor(x => x.Description)
				.SetValidator(new CategoryDescriptionValidator());

			RuleFor(x => x.Name)
				.SetValidator(new CategoryNameValidator());

			RuleFor(x => x).Custom((category, context) =>
			{
				if(
					string.IsNullOrWhiteSpace(category.Name) &&
					string.IsNullOrWhiteSpace(category.Description)
				)
				{
					context.AddFailure("At least one of the fields is required (name, description).");
				}
			});
		}
		// Trimming Strings
		protected override bool PreValidate(ValidationContext<CategoryUpdateDto> context, ValidationResult result)
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
