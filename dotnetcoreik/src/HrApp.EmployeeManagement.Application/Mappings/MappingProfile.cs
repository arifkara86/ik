using AutoMapper;
using HrApp.EmployeeManagement.Application.Features.Employees.Dtos;
using HrApp.EmployeeManagement.Application.Features.Departments.Dtos;
using HrApp.EmployeeManagement.Domain.Entities;
namespace HrApp.EmployeeManagement.Application.Mappings;
public class MappingProfile : Profile {
    public MappingProfile() {
        CreateMap<Employee, EmployeeListVm>()
            .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department != null ? src.Department.Name : null));
        CreateMap<Employee, EmployeeDetailVm>() // EmployeeDetailVm'nin de DepartmentName içerdiğini varsayıyoruz
             .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department != null ? src.Department.Name : null));
        CreateMap<Department, DepartmentDto>();
    }
}