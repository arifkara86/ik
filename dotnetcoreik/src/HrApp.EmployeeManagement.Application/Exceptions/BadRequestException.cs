namespace HrApp.EmployeeManagement.Application.Exceptions;

public class BadRequestException : ApplicationException // Veya direkt Exception
{
    public BadRequestException(string message) : base(message) { }
}