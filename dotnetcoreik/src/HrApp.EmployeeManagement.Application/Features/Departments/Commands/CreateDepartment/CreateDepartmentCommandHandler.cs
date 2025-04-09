// src/HrApp.EmployeeManagement.Application/Features/Departments/Commands/CreateDepartment/CreateDepartmentCommandHandler.cs
using AutoMapper;
using HrApp.EmployeeManagement.Application.Features.Departments.Dtos;
using HrApp.EmployeeManagement.Application.Persistence; // IUnitOfWork için
using HrApp.EmployeeManagement.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HrApp.EmployeeManagement.Application.Features.Departments.Commands.CreateDepartment;

public class CreateDepartmentCommandHandler : IRequestHandler<CreateDepartmentCommand, DepartmentDto>
{
    private readonly IUnitOfWork _unitOfWork; // DEĞİŞTİ
    private readonly IMapper _mapper;
    private readonly ILogger<CreateDepartmentCommandHandler> _logger;

    public CreateDepartmentCommandHandler(
        IUnitOfWork unitOfWork, // DEĞİŞTİ
        IMapper mapper,
        ILogger<CreateDepartmentCommandHandler> logger)
    {
        _unitOfWork = unitOfWork; // DEĞİŞTİ
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<DepartmentDto> Handle(CreateDepartmentCommand request, CancellationToken cancellationToken)
    {
        // Validation Behaviour kontrol ediyor.

        var departmentEntity = new Department(Guid.Empty, request.Name);

        _logger.LogInformation("Adding new department '{DepartmentName}' to database.", request.Name);
        await _unitOfWork.DepartmentRepository.AddAsync(departmentEntity, cancellationToken); // UoW Kullanıldı

        await _unitOfWork.SaveChangesAsync(cancellationToken); // UoW Kullanıldı
        _logger.LogInformation("Database changes saved for new department {DepartmentId}.", departmentEntity.Id);

        _logger.LogInformation("Department '{DepartmentName}' added successfully with ID {DepartmentId}.", request.Name, departmentEntity.Id);
        return _mapper.Map<DepartmentDto>(departmentEntity);
    }
}