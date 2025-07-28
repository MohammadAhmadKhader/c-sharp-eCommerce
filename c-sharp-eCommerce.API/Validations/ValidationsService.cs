using c_sharp_eCommerce.Core.DTOs.Categories;
using c_sharp_eCommerce.Core.DTOs.Orders;
using c_sharp_eCommerce.Core.DTOs.Products;
using c_sharp_eCommerce.Core.DTOs.Users;
using c_sharp_eCommerce.API.Validations.CategoryValidations;
using c_sharp_eCommerce.API.Validations.OrderValidations;
using c_sharp_eCommerce.API.Validations.ProductValidations;
using c_sharp_eCommerce.API.Validations.UserValidations;
using FluentValidation;

namespace c_sharp_eCommerce.API.Services.Validations
{
	public static class ValidationsService
	{
		public static void AddValidators(this IServiceCollection services)
		{
			// product
			services.AddTransient<IValidator<ProductCreateDto>, ProductCreateDtoValidator>();
			services.AddTransient<IValidator<ProductUpdateDto>, ProductUpdateDtoValidator>();

			// cateogry
			services.AddTransient<IValidator<CategoryCreateDto>, CategoryCreateDtoValidator>();
			services.AddTransient<IValidator<CategoryUpdateDto>, CategoryUpdateDtoValidator>();

			// order
			services.AddTransient<IValidator<OrderCreateDto>, OrderCreateDtoValidator>();

			// user
			services.AddTransient<IValidator<LoginRequestDto>, LoginRequestDtoValidator>();
			services.AddTransient<IValidator<RegisterationRequestDto>, RegisterationRequestDtoValidator>();
			services.AddTransient<IValidator<SendEmailDto>, SendEmailDtoValidator>();
			services.AddTransient<IValidator<ResetPasswordDto>, ResetPasswordDtoValidator>();
		}
	}
}
