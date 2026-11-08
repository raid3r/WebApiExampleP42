using AutoMapper;
using WebApiExampleP42.Models.DTO;
using WebApiExampleP42.Models;

namespace WebApiExampleP42.Models.Mapping;

public class UserRegisterRequestToUserMappingProfile: Profile
{
    public UserRegisterRequestToUserMappingProfile()
    {
        CreateMap<UserRegisterRequest, User>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password));
    }
}
