using AutoMapper;
using c_shap_eCommerce.Core.DTOs.Categories;
using c_shap_eCommerce.Core.DTOs.OrderDetails;
using c_shap_eCommerce.Core.DTOs.Orders;
using c_shap_eCommerce.Core.DTOs.Products;
using c_shap_eCommerce.Core.DTOs.Users;
using c_shap_eCommerce.Core.Models;

namespace c_sharp_eCommerce.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // TODO: it returns category as null when the category exist, apply correct loading
            CreateMap<Product, ProductDto>();

            CreateMap<Product, ProductResponseDto>()
                .ForMember(dest => dest.Category, (opt => opt.MapFrom(src => src.Category == null ? null : src.Category.Name)));
			CreateMap<ProductCreateDto, Product>()
                .ForMember(dest => dest.Category, (opt => opt.Ignore())).ReverseMap();
			
			CreateMap<CategoryDto, Category>()
                .ForMember(dest => dest.Products, (opt => opt.Ignore())).ReverseMap();

			CreateMap<Category, CategoryCreateDto>().ReverseMap();

			CreateMap<CategoryDto, CategoryUpdateDto>().ReverseMap();
			CreateMap<User, UserDto>().ReverseMap();
            CreateMap<OrderDetailsDto, OrderDetails>().ReverseMap();

            CreateMap<OrderDto, Order>()
                .ForMember(dest => dest.User, options => options.Ignore())
                .ForMember(dest => dest.OrderDetails, options => options.Ignore())
                .ReverseMap();
		}
    }
}
