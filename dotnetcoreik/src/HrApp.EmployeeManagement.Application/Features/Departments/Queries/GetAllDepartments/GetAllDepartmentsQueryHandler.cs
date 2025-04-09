// src/HrApp.EmployeeManagement.Application/Features/Departments/Queries/GetAllDepartments/GetAllDepartmentsQueryHandler.cs
using AutoMapper;
using HrApp.EmployeeManagement.Application.Features.Departments.Dtos;
using HrApp.EmployeeManagement.Application.Persistence;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HrApp.EmployeeManagement.Application.Features.Departments.Queries.GetAllDepartments;

public class GetAllDepartmentsQueryHandler : IRequestHandler<GetAllDepartmentsQuery, IReadOnlyList<DepartmentDto>>
{
    private readonly IDepartmentRepository _departmentRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetAllDepartmentsQueryHandler> _logger;

    public GetAllDepartmentsQueryHandler(IDepartmentRepository departmentRepository, IMapper mapper, ILogger<GetAllDepartmentsQueryHandler> logger)
    {
        _departmentRepository = departmentRepository;
        _mapper = mapper;
         _logger = logger;
   }

    public async Task<IReadOnlyList<DepartmentDto>> Handle(GetAllDepartmentsQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Retrieving all departments.");
        var departments = await _departmentRepository.GetAllAsync(cancellationToken);

        // Entity listesini DTO listesine map et
        var departmentDtos = _mapper.Map<IReadOnlyList<DepartmentDto>>(departments);
         _logger.LogInformation("Successfully retrieved {DepartmentCount} departments.", departmentDtos.Count);

        return departmentDtos;
    }
}