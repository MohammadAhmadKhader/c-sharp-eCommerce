using c_shap_eCommerce.Core.DTOs.Users;
using c_sharp_eCommerce.Validations.ValidatorsExtensions;
using FluentValidation;

namespace c_sharp_eCommerce.Validations.UserValidations
{
	public class SendEmailDtoValidator : AbstractValidator<SendEmailDto>
	{
		public SendEmailDtoValidator()
		{
			RuleFor(x => x.Email)
				.CustomEmailValidator();
		}
	}
}
