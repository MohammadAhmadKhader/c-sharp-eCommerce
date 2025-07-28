using c_sharp_eCommerce.Core.DTOs.Users;
using c_sharp_eCommerce.API.Validations.ValidatorsExtensions;
using FluentValidation;
using FluentValidation.Results;
namespace c_sharp_eCommerce.API.Validations.UserValidations
{
	public class LoginRequestDtoValidator : AbstractValidator<LoginRequestDto>
	{
		public LoginRequestDtoValidator()
		{
			RuleFor(x => x.Email)
				.CustomEmailValidator();

			RuleFor(x => x.Password)
				.CustomPasswordValidator();
		}

		protected override bool PreValidate(ValidationContext<LoginRequestDto> context, ValidationResult result)
		{
			var instance = context.InstanceToValidate;
			if (instance != null)
			{
				if (instance.Email != null)
				{
					context.InstanceToValidate.Email = instance.Email.Trim();
				}
				if (instance.Password != null)
				{
					context.InstanceToValidate.Password = instance.Password.Trim();
				}

			}
			return base.PreValidate(context, result);
		}
	}
}
