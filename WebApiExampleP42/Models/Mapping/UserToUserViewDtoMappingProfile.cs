using AutoMapper;
using WebApiExampleP42.Models.DTO;

namespace WebApiExampleP42.Models.Mapping;

public class UserToUserViewDtoMappingProfile : Profile
{
    public UserToUserViewDtoMappingProfile()
    {
        CreateMap<User, UserViewDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
            .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.RoleId))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt));
    }
}
