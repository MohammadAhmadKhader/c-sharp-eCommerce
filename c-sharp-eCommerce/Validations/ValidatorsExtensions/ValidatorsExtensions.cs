using FluentValidation;

namespace c_sharp_eCommerce.Validations.ValidatorsExtensions
{
	public static class ValidatorsExtensions
	{
		public static IRuleBuilderOptions<T, string> CustomEmailValidator<T>(this IRuleBuilder<T, string> rule)
		{
			return rule
				.NotEmpty().WithMessage("email can not be empty")
				.EmailAddress().WithMessage("invalid email")
				.MinimumLength(6).WithMessage("minimum email length must be 6")
				.MaximumLength(64).WithMessage("maximum email length must be 64");
		}

		public static IRuleBuilderOptions<T, string> CustomPasswordValidator<T>(this IRuleBuilder<T, string> rule)
		{
			return rule
				.NotEmpty().WithMessage("email can not be empty")
				.MinimumLength(6).WithMessage("minimum password length must be 6")
				.MaximumLength(32).WithMessage("maximum password length must be 32");
		}

		public static IRuleBuilderOptions<T, IFormFile?> CustomImageValidator<T>(this IRuleBuilder<T, IFormFile?> rule)
		{
			return rule
				.Must(file => file == null || file.Length > 0).WithMessage("file can not be empty")
				.Must(file => file == null || file.Length <= 10 * 1024 * 1024).WithMessage("file can not exceed 10 mb size");
		}
	}

}
