using FluentValidation;

namespace c_sharp_eCommerce.API.Validations.ProductValidations
{
	public class ProductValidators
	{
		public class ProductQuantityValidator : AbstractValidator<int?>
		{
			public ProductQuantityValidator()
			{
				RuleFor(x => x)
					.InclusiveBetween(0, 100000).WithMessage("quantity must be between 0 and 100000");
			}
		}

		public class ProductCategoryIdValidator : AbstractValidator<int?>
		{
			public ProductCategoryIdValidator()
			{
				RuleFor(x => x)
					.GreaterThan(0).WithMessage("category id must be greater than 1")
					.LessThan(int.MaxValue).WithMessage($"category id must be less than {int.MaxValue}"); ;
			}
		}

		public class ProductPriceValidator : AbstractValidator<double?>
		{
			public ProductPriceValidator()
			{
				RuleFor(x => x)
				.InclusiveBetween(0.0, 10000).WithMessage("price must be between 0 and 10000");
			}
		}
		public class ProductDescriptionValidator : AbstractValidator<string?>
		{
			public ProductDescriptionValidator()
			{
				RuleFor(x => x)
					.Length(6, 256).WithMessage("description length must be between 6 and 256");
			}
		}
		public class ProductNameValidator : AbstractValidator<string?>
		{
			public ProductNameValidator()
			{
				RuleFor(x => x)
					.Length(2, 32).WithMessage("name length must be between 2 and 32");
			}
		}
	}
}
