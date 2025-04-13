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
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateDepartmentCommandHandler> _logger;

    public CreateDepartmentCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<CreateDepartmentCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<DepartmentDto> Handle(CreateDepartmentCommand request, CancellationToken cancellationToken)
    {
        // FluentValidation behavior'ı zaten isim benzersizliğini kontrol etmeli (CreateDepartmentCommandValidator içinde)

        var departmentEntity = new Department(Guid.Empty, request.Name); // Yeni ID constructor'da oluşur

        _logger.LogInformation("Attempting to add new department '{DepartmentName}' via repository.", request.Name);
        // 1. Adım: Repository'ye ekle (EF Core takip etmeye başlar)
        await _unitOfWork.DepartmentRepository.AddAsync(departmentEntity, cancellationToken);
        _logger.LogInformation("Entity for department '{DepartmentName}' added to DbContext.", request.Name);


        // --- KESİN OLMASI GEREKEN ADIM ---
        // 2. Adım: Değişiklikleri veritabanına kaydet
        try
        {
            var result = await _unitOfWork.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("SaveChangesAsync completed for new department. Result: {Result}", result);
            if (result <= 0)
            {
                 // Eğer SaveChangesAsync 0 veya daha az dönerse, kayıt yapılmamış demektir.
                 // Bu normalde bir sorun işaretidir, loglayıp hata fırlatabiliriz.
                 _logger.LogError("SaveChangesAsync returned {Result}, indicating no changes were saved to the database for department '{DepartmentName}'.", result, request.Name);
                 // throw new Exception("Failed to save new department to the database."); // Veya daha spesifik bir exception
            }
        }
        catch (Exception ex)
        {
             _logger.LogError(ex, "An error occurred during SaveChangesAsync for new department '{DepartmentName}'.", request.Name);
             throw; // Hatanın yukarı katmanlara gitmesine izin ver (Global Exception Handler yakalar)
        }
        // --- Bitiş: KESİN OLMASI GEREKEN ADIM ---


        _logger.LogInformation("Department '{DepartmentName}' added successfully with ID {DepartmentId}.", request.Name, departmentEntity.Id);
        // Kaydedilen entity'yi DTO'ya map et
        return _mapper.Map<DepartmentDto>(departmentEntity);
    }
}