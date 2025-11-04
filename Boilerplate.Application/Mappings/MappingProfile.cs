using Boilerplate.Application.Features.Users;
using Boilerplate.Application.Features.Users.Create;
using Boilerplate.Domain.Entities;
using AutoMapper;

namespace Boilerplate.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserResponse>();
            CreateMap<CreateUserCommand, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore());
        }
    }
}
