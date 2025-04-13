using AutoMapper;
using HrApp.EmployeeManagement.Application.Features.Employees.Dtos;
using HrApp.EmployeeManagement.Application.Features.Departments.Dtos;
using HrApp.EmployeeManagement.Domain.Entities;
// Diğer Command/Query DTO usingleri eklenebilir

namespace HrApp.EmployeeManagement.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Department -> DepartmentDto
        CreateMap<Department, DepartmentDto>();

        // Employee -> EmployeeListVm (DepartmentName Handler'da maplenecek)
        CreateMap<Employee, EmployeeListVm>();
        // .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department != null ? src.Department.Name : null)); // Handler'da yapılacak

        // Employee -> EmployeeDetailVm (Varsa)
         CreateMap<Employee, EmployeeDetailVm>() // EmployeeDetailVm tanımlıysa
              .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department != null ? src.Department.Name : null));
    }
}