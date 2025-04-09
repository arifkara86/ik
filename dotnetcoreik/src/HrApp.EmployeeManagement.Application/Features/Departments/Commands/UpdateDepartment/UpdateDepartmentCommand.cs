// src/HrApp.EmployeeManagement.Application/Features/Departments/Commands/UpdateDepartment/UpdateDepartmentCommand.cs
using MediatR; // IRequest için (Unit: işlem sonucu veri döndürmeyecekse)

namespace HrApp.EmployeeManagement.Application.Features.Departments.Commands.UpdateDepartment;

// Bu komut işlendiğinde bir sonuç dönmesi gerekmiyorsa IRequest<Unit> veya sadece IRequest kullanılır.
// Genellikle Update işlemlerinde bir şey dönülmez (Başarılıysa 204 No Content).
public class UpdateDepartmentCommand : IRequest // veya IRequest<Unit>
{
    public Guid Id { get; set; } // Hangi departmanın güncelleneceği
    public string Name { get; set; } = string.Empty; // Yeni adı
}