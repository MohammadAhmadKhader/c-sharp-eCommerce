using c_shap_eCommerce.Core.DTOs.Products;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace c_sharp_eCommerce.Services.Validations
{
	public class ProductCreateDtoValidator : AbstractValidator<ProductCreateDto>
	{
		public ProductCreateDtoValidator()
		{
			
		}
	}
}
