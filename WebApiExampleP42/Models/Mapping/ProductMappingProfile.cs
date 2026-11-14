using AutoMapper;
using WebApiExampleP42.Models.DTO;

namespace WebApiExampleP42.Models.Mapping;

public class ProductMappingProfile: Profile
{
    public ProductMappingProfile()
    {
        CreateMap<Product, ProductView>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price));
    }

}
