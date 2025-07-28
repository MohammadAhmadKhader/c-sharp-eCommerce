using c_sharp_eCommerce.Core.DTOs.Users;
using c_sharp_eCommerce.API.Validations.ValidatorsExtensions;
using FluentValidation;

namespace c_sharp_eCommerce.API.Validations.UserValidations
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
