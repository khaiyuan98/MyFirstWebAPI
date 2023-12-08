using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using System.Security.AccessControl;
using Test.DataAccess.Models;
using Test.DataAccess.Models.Users;
using Test.WebAPI.Models.Auth;
using Test.WebAPI.Models.Department;
using Test.WebAPI.Models.Employee;
using Test.WebAPI.Models.User;

namespace Test.WebAPI
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<NewDepartmentDto, Department>().ReverseMap();
            CreateMap<NewEmployeeDto, Employee>().ReverseMap();
            CreateMap<CurrentUserDto, FullUser>().ReverseMap();
            CreateMap<AuthUser, FullUser>().ReverseMap();

            CreateMap<User, GetUserDto>();
            CreateMap<UserRole, UserRoleDto>();
            CreateMap<UserGroup, UserGroupDto>();
            CreateMap<NewUserDto, AddUser>();
            CreateMap<UpdateUserDto, UpdateUser>();

            CreateMap<FullUser, UserDetailsDto>()
                .ForMember(dest => dest.LastUpdatedBy,
                opt => opt.MapFrom((src, dest) => src.LastUpdatedBy?.Username)
                );

            //CreateMap<ContractRequestModel, ContractModel>()
            //    .ForPath(dest => dest.Company.id, o => o.MapFrom(c => c.company_id))
            //    .ForPath(dest => dest.ServiceRate.id, o => o.MapFrom(c => c.service_rate_id))
            //    .ReverseMap();

        }
    }
}
