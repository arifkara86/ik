// src/HrApp.EmployeeManagement.Application/Features/Departments/Queries/GetDepartmentById/GetDepartmentByIdQueryHandler.cs
using AutoMapper;
using HrApp.EmployeeManagement.Application.Exceptions; // NotFoundException için
using HrApp.EmployeeManagement.Application.Features.Departments.Dtos;
using HrApp.EmployeeManagement.Application.Persistence;
using HrApp.EmployeeManagement.Domain.Entities; // Department entity tipi için
using MediatR;
using Microsoft.Extensions.Logging;

namespace HrApp.EmployeeManagement.Application.Features.Departments.Queries.GetDepartmentById;

public class GetDepartmentByIdQueryHandler : IRequestHandler<GetDepartmentByIdQuery, DepartmentDto?> // Nullable Dto
{
    private readonly IDepartmentRepository _departmentRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetDepartmentByIdQueryHandler> _logger;

    public GetDepartmentByIdQueryHandler(IDepartmentRepository departmentRepository, IMapper mapper, ILogger<GetDepartmentByIdQueryHandler> logger)
    {
        _departmentRepository = departmentRepository;
        _mapper = mapper;
         _logger = logger;
    }

    public async Task<DepartmentDto?> Handle(GetDepartmentByIdQuery request, CancellationToken cancellationToken)
    {
         _logger.LogInformation("Retrieving department with ID: {DepartmentId}", request.Id);
        var department = await _departmentRepository.GetByIdAsync(request.Id, cancellationToken);

        if (department == null)
        {
            // Bulunamadıysa null dönmek yerine özel bir Exception fırlatmak daha iyi olabilir.
            // Controller veya bir Exception Handling Middleware bu Exception'ı yakalayıp 404 dönebilir.
             _logger.LogWarning("Department with ID: {DepartmentId} not found.", request.Id);
            // return null; // Veya Exception fırlat:
             throw new NotFoundException(nameof(Department), request.Id);
        }

         _logger.LogInformation("Successfully retrieved department with ID: {DepartmentId}", request.Id);
        return _mapper.Map<DepartmentDto>(department);
    }
}