using AutoMapper;
using WebApiExampleP42.Models.DTO;

namespace WebApiExampleP42.Models.Mapping;

public class PaginatedResultMappingProfile : Profile
{
    public PaginatedResultMappingProfile()
    {
        CreateMap(typeof(PaginatedResult<>), typeof(PaginatedResult<>));
    }
}

