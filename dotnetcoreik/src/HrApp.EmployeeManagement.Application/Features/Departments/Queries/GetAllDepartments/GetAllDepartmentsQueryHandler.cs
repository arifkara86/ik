// src/HrApp.EmployeeManagement.Application/Features/Departments/Queries/GetAllDepartments/GetAllDepartmentsQueryHandler.cs
using AutoMapper;
using HrApp.EmployeeManagement.Application.Features.Departments.Dtos;
using HrApp.EmployeeManagement.Application.Persistence;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Linq; // Select için

namespace HrApp.EmployeeManagement.Application.Features.Departments.Queries.GetAllDepartments;

public class GetAllDepartmentsQueryHandler : IRequestHandler<GetAllDepartmentsQuery, IReadOnlyList<DepartmentDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<GetAllDepartmentsQueryHandler> _logger;

    public GetAllDepartmentsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetAllDepartmentsQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IReadOnlyList<DepartmentDto>> Handle(GetAllDepartmentsQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("GetAllDepartmentsQueryHandler: Retrieving all departments from repository.");
        // Doğrudan repository'den tüm departmanları çek
        var departments = await _unitOfWork.DepartmentRepository.GetAllAsync(cancellationToken); // İlişkisiz, basit GetAll yeterli
        _logger.LogInformation("GetAllDepartmentsQueryHandler: Retrieved {Count} departments from repository.", departments.Count);

        // AutoMapper ile DTO listesine çevir
        var departmentDtos = _mapper.Map<IReadOnlyList<DepartmentDto>>(departments);
        _logger.LogInformation("GetAllDepartmentsQueryHandler: Mapped {Count} departments to DTOs.", departmentDtos.Count);

        return departmentDtos;
    }
}