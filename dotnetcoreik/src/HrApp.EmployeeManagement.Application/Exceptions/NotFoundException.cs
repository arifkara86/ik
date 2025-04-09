namespace HrApp.EmployeeManagement.Application.Exceptions;

public class NotFoundException : ApplicationException // Veya direkt Exception
{
    public NotFoundException(string name, object key)
        : base($"{name} ({key}) was not found.")
    {
    }
}