using AutoMapper;
using UserManagementSystem.DTOs;
using UserManagementSystem.Models;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<User, UserDto>()
            .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role.RoleName)); 
        CreateMap<Role, RoleDto>().ReverseMap();
        CreateMap<UserDto, User>();
    }
}
