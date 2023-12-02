using AutoMapper;
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

            //CreateMap<ContactRequestModel, ContactModel>()
            //    .ForPath(dest => dest.Company.id, o => o.MapFrom(c => c.company_id))
            //    .ForPath(dest => dest.BusinessPartner.id, o => o.MapFrom(c => c.business_partner_id))
            //    .ReverseMap();

            //CreateMap<ContractRequestModel, ContractModel>()
            //    .ForPath(dest => dest.Company.id, o => o.MapFrom(c => c.company_id))
            //    .ForPath(dest => dest.ServiceRate.id, o => o.MapFrom(c => c.service_rate_id))
            //    .ReverseMap();

        }
    }
}
