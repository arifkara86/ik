// src/HrApp.EmployeeManagement.Application/Features/Departments/Commands/CreateDepartment/CreateDepartmentCommand.cs
using HrApp.EmployeeManagement.Application.Features.Departments.Dtos; // DepartmentDto için
using MediatR; // IRequest için

namespace HrApp.EmployeeManagement.Application.Features.Departments.Commands.CreateDepartment;

// Bu komut işlendiğinde bir DepartmentDto dönecek (oluşturulan departmanın bilgisi)
public class CreateDepartmentCommand : IRequest<DepartmentDto>
{
    // Komutun taşıdığı veri (CreateDepartmentDto'dan farklı olabilir ama genelde benzerdir)
    public string Name { get; set; } = string.Empty;

    // DTO'dan Command'a dönüşüm için constructor veya AutoMapper kullanılabilir.
    // Şimdilik basit tutalım, Controller'da manuel atama yaparız veya Handler'da alırız.
}