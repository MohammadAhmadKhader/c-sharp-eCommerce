using c_sharp_eCommerce.Core.DTOs.Categories;
using FluentValidation;
using FluentValidation.Results;
using static c_sharp_eCommerce.API.Validations.CategoryValidations.CategoryValidators;

namespace c_sharp_eCommerce.API.Validations.CategoryValidations
{
	public class CategoryCreateDtoValidator : AbstractValidator<CategoryCreateDto>
	{
		public CategoryCreateDtoValidator()
		{
			RuleFor(x => x.Description)
				.NotEmpty().WithMessage("category description is required")
				.SetValidator(new CategoryDescriptionValidator());

			RuleFor(x => x.Name)
				.NotEmpty().WithMessage("category name is required")
				.SetValidator(new CategoryNameValidator());

		}
		protected override bool PreValidate(ValidationContext<CategoryCreateDto> context, ValidationResult result)
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
