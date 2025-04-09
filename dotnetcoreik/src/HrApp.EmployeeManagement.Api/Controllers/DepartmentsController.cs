// src/HrApp.EmployeeManagement.Api/Controllers/DepartmentsController.cs

using HrApp.EmployeeManagement.Application.Features.Departments.Commands.CreateDepartment;
using HrApp.EmployeeManagement.Application.Features.Departments.Commands.DeleteDepartment;
using HrApp.EmployeeManagement.Application.Features.Departments.Commands.UpdateDepartment;
using HrApp.EmployeeManagement.Application.Features.Departments.Dtos;
using HrApp.EmployeeManagement.Application.Features.Departments.Queries.GetAllDepartments;
using HrApp.EmployeeManagement.Application.Features.Departments.Queries.GetDepartmentById;
using MediatR; // IMediator için
using Microsoft.AspNetCore.Mvc; // ControllerBase, IActionResult vb. için

namespace HrApp.EmployeeManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DepartmentsController : ControllerBase
{
    private readonly IMediator _mediator; // MediatR'ı enjekte et
    private readonly ILogger<DepartmentsController> _logger;

    public DepartmentsController(IMediator mediator, ILogger<DepartmentsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    // GET: api/departments
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<DepartmentDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IReadOnlyList<DepartmentDto>>> GetAllDepartments(CancellationToken cancellationToken)
    {
        // Sorguyu MediatR'a gönder
        var query = new GetAllDepartmentsQuery();
        var departments = await _mediator.Send(query, cancellationToken);
        return Ok(departments);
    }

    // GET: api/departments/{id}
    [HttpGet("{id:guid}", Name = "GetDepartmentById")] // Route'a isim verdik (CreatedAtAction için)
    [ProducesResponseType(typeof(DepartmentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<DepartmentDto>> GetDepartmentById(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetDepartmentByIdQuery(id);
        var department = await _mediator.Send(query, cancellationToken);
        // NotFoundException fırlatıldığı için burada null kontrolüne gerek yok (eğer global handler varsa)
        // Eğer global handler yoksa, try-catch ile NotFoundException yakalanıp NotFound() dönülebilir.
        return Ok(department);
    }

    // POST: api/departments
    [HttpPost]
    [ProducesResponseType(typeof(DepartmentDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)] // FluentValidation hataları için
    [ProducesResponseType(StatusCodes.Status409Conflict)] // Benzersizlik hatası için
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<DepartmentDto>> CreateDepartment([FromBody] CreateDepartmentDto createDto, CancellationToken cancellationToken)
    {
        // DTO'dan Command'a manuel map'leme (veya AutoMapper kullanılabilir)
        var command = new CreateDepartmentCommand { Name = createDto.Name };

        // Komutu MediatR'a gönder
        var createdDepartmentDto = await _mediator.Send(command, cancellationToken);

        // Başarılı oluşturmada 201 Created yanıtı dön (yeni kaynağın adresi ve nesnesi ile)
        return CreatedAtAction(nameof(GetDepartmentById), new { id = createdDepartmentDto.Id }, createdDepartmentDto);
    }

    // PUT: api/departments/{id}
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)] // ID uyuşmazlığı veya validation
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)] // Benzersizlik hatası için
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateDepartment(Guid id, [FromBody] UpdateDepartmentDto updateDto, CancellationToken cancellationToken)
    {
         // URL'deki ID ile Body'deki Name'i kullanarak Command oluştur
         var command = new UpdateDepartmentCommand
         {
             Id = id,
             Name = updateDto.Name
         };

        // Komutu MediatR'a gönder (Sonuç beklemiyoruz - IRequest)
        await _mediator.Send(command, cancellationToken);

        return NoContent(); // Başarılı güncellemede 204 No Content
    }

     // DELETE: api/departments/{id}
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteDepartment(Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteDepartmentCommand(id);
        await _mediator.Send(command, cancellationToken);
        return NoContent(); // Başarılı silmede 204 No Content
    }
}