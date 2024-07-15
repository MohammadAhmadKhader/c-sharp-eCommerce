using AutoMapper;
using c_shap_eCommerce.Core.DTOs.Categories;
using c_shap_eCommerce.Core.DTOs.Products;
using c_shap_eCommerce.Core.Models;

namespace c_sharp_eCommerce.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductDTO>()
                .ForMember(Source => Source.Category, options => options.MapFrom(source => source.Category != null ? source.Name : null));
            // ReverseMap() method to change all the properties
            CreateMap<Category, CategoryDTO>().ReverseMap();
        }
    }
}
