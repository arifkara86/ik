namespace HrApp.EmployeeManagement.Application.Features.Employees.Dtos;

public class EmployeeListVm
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Position { get; set; }
    public DateTime HireDate { get; set; }
    public string? DepartmentName { get; set; } // Eklendiğinden emin ol
}