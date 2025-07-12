using c_shap_eCommerce.Core.DTOs.Categories;
using c_shap_eCommerce.Core.DTOs.Orders;
using c_shap_eCommerce.Core.DTOs.Products;
using c_shap_eCommerce.Core.DTOs.Users;
using c_sharp_eCommerce.Validations.CategoryValidations;
using c_sharp_eCommerce.Validations.OrderValidations;
using c_sharp_eCommerce.Validations.ProductValidations;
using c_sharp_eCommerce.Validations.UserValidations;
using FluentValidation;

namespace c_sharp_eCommerce.Services.Validations
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