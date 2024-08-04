using AutoMapper;
using c_shap_eCommerce.Core.DTOs.Categories;
using c_shap_eCommerce.Core.DTOs.Products;
using c_shap_eCommerce.Core.DTOs.Users;
using c_shap_eCommerce.Core.Models;

namespace c_sharp_eCommerce.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductDto>()
                .ForMember(destination => destination.Category, options => options.MapFrom(source => source.Category != null ? source.Category.Name : null));
            CreateMap<Product, ProductCreateDto>().ReverseMap();
			// ReverseMap() method to change all the properties
			CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<User, UserDto>().ReverseMap();
        }
    }
}
