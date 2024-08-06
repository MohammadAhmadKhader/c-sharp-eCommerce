using c_shap_eCommerce.Core.DTOs.Users;
using c_sharp_eCommerce.Validations.ValidatorsExtensions;
using FluentValidation;
using FluentValidation.Results;

namespace c_sharp_eCommerce.Validations.UserValidations
{
	public class RegisterationRequestDtoValidator : AbstractValidator<RegisterationRequestDto>
	{
        public RegisterationRequestDtoValidator()
        {

			RuleFor(x => x.Email)
				.CustomEmailValidator();

			RuleFor(x => x.Password)
				.CustomPasswordValidator();

			RuleFor(x => x.FirstName)
				.NotEmpty().WithMessage("first name can not be empty")
				.MinimumLength(3).WithMessage("minimum first name length must be 3")
				.MaximumLength(50).WithMessage("maximum first name length must be 50");

			RuleFor(x => x.LastName)
				.NotEmpty().WithMessage("last name can not be empty")
				.MinimumLength(3).WithMessage("minimum last name length must be 3")
				.MaximumLength(50).WithMessage("maximum last name length must be 50");

		}
		protected override bool PreValidate(ValidationContext<RegisterationRequestDto> context, ValidationResult result)
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
				if (instance.FirstName != null)
				{
					context.InstanceToValidate.FirstName = instance.FirstName.Trim();
				}
				if (instance.LastName != null)
				{
					context.InstanceToValidate.LastName = instance.LastName.Trim();
				}

			}
			return base.PreValidate(context, result);
		}
	}
}
