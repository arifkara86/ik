namespace HrApp.EmployeeManagement.Application.Features.Employees.Dtos;

public class EmployeeDetailVm // Bu DTO'nun GetEmployeeByIdQueryHandler tarafından döndürüldüğünü varsayıyoruz
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public DateTime HireDate { get; set; }
    public string? Position { get; set; }
    public decimal Salary { get; set; }
    public Guid? DepartmentId { get; set; }
    public string? DepartmentName { get; set; } // Doğru yerde olduğundan emin ol
}