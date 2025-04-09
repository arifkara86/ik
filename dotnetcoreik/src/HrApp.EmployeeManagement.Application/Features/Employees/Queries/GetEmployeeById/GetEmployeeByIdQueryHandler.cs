// HrApp.EmployeeManagement.Application/Features/Employees/Queries/GetEmployeeById/GetEmployeeByIdQueryHandler.cs

using AutoMapper; // IMapper için EKLENDİ
using HrApp.EmployeeManagement.Application.Features.Employees.Dtos; // EmployeeDetailVm için EKLENDİ
using HrApp.EmployeeManagement.Application.Persistence;
using HrApp.EmployeeManagement.Domain.Entities; // Map kaynağı için
using MediatR;
using Microsoft.Extensions.Logging;

// Namespace DTO'ları içermiyor, DTO using'i eklendi
namespace HrApp.EmployeeManagement.Application.Features.Employees.Queries.GetEmployeeById;

// --- DEĞİŞİKLİK: Dönüş tipi artık EmployeeDetailVm? ---
public class GetEmployeeByIdQueryHandler : IRequestHandler<GetEmployeeByIdQuery, EmployeeDetailVm?>
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IMapper _mapper; // AutoMapper için EKLENDİ
    private readonly ILogger<GetEmployeeByIdQueryHandler> _logger;

    // --- DEĞİŞİKLİK: Constructor'a IMapper eklendi ---
    public GetEmployeeByIdQueryHandler(
        IEmployeeRepository employeeRepository,
        IMapper mapper, // EKLENDİ
        ILogger<GetEmployeeByIdQueryHandler> logger)
    {
        _employeeRepository = employeeRepository ?? throw new ArgumentNullException(nameof(employeeRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper)); // EKLENDİ
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    // --- DEĞİŞİKLİK: Handle metodu DTO dönecek ve Mapper kullanacak ---
    public async Task<EmployeeDetailVm?> Handle(GetEmployeeByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling GetEmployeeByIdQuery for ID: {EmployeeId}", request.Id);

        try
        {
            var employee = await _employeeRepository.GetByIdAsync(request.Id, cancellationToken);

            if (employee == null)
            {
                 _logger.LogWarning("Employee with ID: {EmployeeId} not found in repository.", request.Id);
                return null; // Bulunamadıysa null dönmeye devam
            }

            // AutoMapper kullanarak Employee entity'sini EmployeeDetailVm DTO'suna dönüştür
            var employeeVm = _mapper.Map<EmployeeDetailVm>(employee); // EKLENDİ

            _logger.LogInformation("Successfully retrieved and mapped employee with ID: {EmployeeId}", request.Id);
            return employeeVm; // DTO'yu dön
        }
        catch (Exception ex)
        {
             _logger.LogError(ex, "Error occurred while handling GetEmployeeByIdQuery for ID: {EmployeeId}", request.Id);
            throw;
        }
    }
}