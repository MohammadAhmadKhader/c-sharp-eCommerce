using AutoMapper;
using c_shap_eCommerce.Core.DTOs;
using c_shap_eCommerce.Core.Models;

namespace c_sharp_eCommerce.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductDTO>()
                .ForMember(destination => destination.Category, option => option.MapFrom(source => source.Category != null ? source.Name : null));
            // ReverseMap() method to change all the properties
        }
    }
}
