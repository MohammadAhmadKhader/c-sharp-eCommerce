using FluentValidation;

namespace c_sharp_eCommerce.Validations.CategoryValidations
{
	public class CategoryValidators
	{
		public class CategoryDescriptionValidator : AbstractValidator<string?> 
		{
            public CategoryDescriptionValidator()
            {
				RuleFor(x => x)
				.Length(4, 1024).WithMessage("description length must be between 3 and 1024 characters");

			}
		}

		public class CategoryNameValidator : AbstractValidator<string?>
		{
            public CategoryNameValidator()
            {
				RuleFor(x => x)
				.Length(2, 64).WithMessage("name length must be between 2 and 64 characters");
			}
        }

	}
}
