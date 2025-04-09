// src/HrApp.EmployeeManagement.Api/Controllers/EmployeesController.cs

// --- Önceki using ifadeleri ---
using HrApp.EmployeeManagement.Application.Features.Employees.Commands.CreateEmployee;
using HrApp.EmployeeManagement.Application.Features.Employees.Commands.DeleteEmployee;
using HrApp.EmployeeManagement.Application.Features.Employees.Commands.UpdateEmployee;
using HrApp.EmployeeManagement.Application.Features.Employees.Dtos;
using HrApp.EmployeeManagement.Application.Features.Employees.Queries.GetAllEmployees;
using HrApp.EmployeeManagement.Application.Features.Employees.Queries.GetEmployeeById;
using MediatR;
using HrApp.EmployeeManagement.Application.Features.Employees.Dtos; // Bu satırı ekle
using Microsoft.AspNetCore.Mvc;
// EmployeeUpdateModel ve EmployeeCreateModel için using (muhtemelen aynı dosyada veya Dtos altında)
// using HrApp.EmployeeManagement.Api.Models; // VEYA ilgili DTO namespace'i


namespace HrApp.EmployeeManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<EmployeesController> _logger;
    // DbContext ve Repository'ler artık burada YOK (CQRS kullanıyoruz)

    public EmployeesController(IMediator mediator, ILogger<EmployeesController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    // GET: api/employees
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<EmployeeListVm>), StatusCodes.Status200OK)] // DTO tipi güncellendi
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<EmployeeListVm>>> GetAllEmployees(CancellationToken cancellationToken)
    {
        var query = new GetAllEmployeesQuery();
        var employees = await _mediator.Send(query, cancellationToken);
        return Ok(employees);
    }

    // GET: api/employees/{id}
    [HttpGet("{id:guid}", Name = "GetEmployeeById")] // Route adı eklendi
    [ProducesResponseType(typeof(EmployeeDetailVm), StatusCodes.Status200OK)] // DTO tipi güncellendi
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<EmployeeDetailVm>> GetEmployeeById(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetEmployeeByIdQuery(id);
        var employee = await _mediator.Send(query, cancellationToken);
        // Handler NotFoundException fırlatıyor, global handler yakalar.
        return Ok(employee);
    }

    // POST: api/employees
    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)] // ID dönecek
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)] // Email unique hatası
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Guid>> CreateEmployee([FromBody] CreateEmployeeDto createDto, CancellationToken cancellationToken) // DTO kullanılıyor
    {
        // --- DEĞİŞİKLİK: DTO'dan Command'a map'leme düzeltildi ---
        var command = new CreateEmployeeCommand
        {
            FirstName = createDto.FirstName,
            LastName = createDto.LastName,
            Email = createDto.Email,
            DateOfBirth = createDto.DateOfBirth,
            HireDate = createDto.HireDate,
            Position = createDto.Position,
            DepartmentId = createDto.DepartmentId, // DepartmentId kullanılıyor
            Salary = createDto.Salary // Salary kullanılıyor
        };
        // --- Bitiş: DEĞİŞİKLİK ---

        var employeeId = await _mediator.Send(command, cancellationToken);

        // CreatedAtAction ile yeni oluşturulan kaynağın ID'sini ve adresini dön
        return CreatedAtAction(nameof(GetEmployeeById), new { id = employeeId }, employeeId);
    }

    // PUT: api/employees/{id}
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)] // Email unique hatası
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateEmployee(Guid id, [FromBody] UpdateEmployeeDto updateDto, CancellationToken cancellationToken) // DTO kullanılıyor
    {
        // ID uyuşmazlığı kontrolü (isteğe bağlı ama iyi pratik)
        // Eğer UpdateEmployeeDto'da Id alanı varsa:
        // if (id != updateDto.Id)
        // {
        //     return BadRequest("ID in URL must match ID in request body.");
        // }

        // --- DEĞİŞİKLİK: DTO'dan Command'a map'leme düzeltildi (Hatanın olduğu yer burasıydı) ---
        var command = new UpdateEmployeeCommand
        {
            Id = id, // URL'den gelen ID
            FirstName = updateDto.FirstName,
            LastName = updateDto.LastName,
            Email = updateDto.Email,
            DateOfBirth = updateDto.DateOfBirth,
            Position = updateDto.Position,
            DepartmentId = updateDto.DepartmentId, // DepartmentId kullanılıyor
            Salary = updateDto.Salary // Salary kullanılıyor
        };
         // --- Bitiş: DEĞİŞİKLİK ---

        await _mediator.Send(command, cancellationToken);

        return NoContent(); // Başarılı güncellemede 204
    }

     // DELETE: api/employees/{id}
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteEmployee(Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteEmployeeCommand(id); // Command'ın constructor'ı ID almalı
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }
}

// === DTO Tanımları (Eğer ayrı dosyada değillerse) ===
// NOT: Bu DTO'ların Application katmanındaki Features/Employees/Dtos altında olması daha iyi olur.
// Eğer oradalarsa buradaki tanımları silip yukarıya doğru using ekleyin.

// public class CreateEmployeeDto { ... } // İçeriği CreateEmployeeCommand ile aynı olmalı
// public class UpdateEmployeeDto { ... } // İçeriği UpdateEmployeeCommand ile aynı olmalı (Id hariç)
// public class EmployeeListVm { ... } // GetAllEmployeesQueryHandler'ın döndüğü DTO
// public class EmployeeDetailVm { ... } // GetEmployeeByIdQueryHandler'ın döndüğü DTO