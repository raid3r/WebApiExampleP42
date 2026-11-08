using WebApiExampleP42.Models.DTO;

namespace WebApiExampleP42.Models.Mapping;

public class UserToUserTokenMappingProfile : AutoMapper.Profile
{
    public UserToUserTokenMappingProfile()
    {
        CreateMap<User, UserToken>()
            .ForMember(dest => dest.Token, opt => opt.MapFrom(src => src.Token));
    }
}
