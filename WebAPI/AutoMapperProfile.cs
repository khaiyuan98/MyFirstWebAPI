using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using System.Security.AccessControl;
using Test.DataAccess.Models;
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
            CreateMap<NewUserDto, User>().ReverseMap();
            CreateMap<CurrentUserDto, User>().ReverseMap();
            CreateMap<AuthUser, User>().ReverseMap();
            CreateMap<UserDto, User>().ReverseMap();

            CreateMap<User, UserDto>()
                .ForMember(dest => dest.RoleName,
                opt => opt.MapFrom((src, dest) => src.UserRole?.Description)
                )
                .ForMember(dest => dest.UserGroups,
                opt => opt.MapFrom((src, dest) => src.UserGroups?.Select(x => x.Description).ToList())
                )
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
