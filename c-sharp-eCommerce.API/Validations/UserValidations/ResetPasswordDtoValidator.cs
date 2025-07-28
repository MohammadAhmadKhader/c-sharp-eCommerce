using c_sharp_eCommerce.Core.DTOs.Users;
using c_sharp_eCommerce.API.Validations.ValidatorsExtensions;
using FluentValidation;

namespace c_sharp_eCommerce.API.Validations.UserValidations
{
	public class ResetPasswordDtoValidator : AbstractValidator<ResetPasswordDto>
	{
		public ResetPasswordDtoValidator()
		{
			RuleFor(x => x.ConfirmPassword)
				.CustomPasswordValidator();

			RuleFor(x => x.NewPassword)
				.CustomPasswordValidator();

			RuleFor(x => x.Email)
				.CustomEmailValidator();

			RuleFor(x => x.Token)
				.NotEmpty().WithMessage("Token is required")
				.Length(20, 200).WithMessage("Invalid token");
		}
	}
}
