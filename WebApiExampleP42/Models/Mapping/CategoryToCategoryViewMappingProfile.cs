using AutoMapper;
using WebApiExampleP42.Models.DTO;

namespace WebApiExampleP42.Models.Mapping;

public class CategoryToCategoryViewMappingProfile : Profile
{
    public CategoryToCategoryViewMappingProfile()
    {
        CreateMap<Category, CategoryView>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
    }
}
