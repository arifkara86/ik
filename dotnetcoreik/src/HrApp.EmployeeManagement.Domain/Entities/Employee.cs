using HrApp.EmployeeManagement.Domain.Entities; // Department için eklendi (veya namespace içinde olduğu için gerekmeyebilir)

namespace HrApp.EmployeeManagement.Domain.Entities;

public class Employee
{
    public Guid Id { get; private set; }
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public DateTime DateOfBirth { get; private set; }
    public string? Position { get; private set; } // Position string olarak kalabilir
    // public string? Department { get; private set; } // Bu alanı kaldırıyoruz, ID ve navigation property kullanacağız
    public DateTime HireDate { get; private set; }
    public decimal Salary { get; private set; }

    // --- Yeni Eklenen Alanlar ---
    // Foreign Key Property: İlişkili Department'ın ID'sini tutar.
    public Guid? DepartmentId { get; private set; }

    // Navigation Property: İlişkili Department nesnesine erişimi sağlar.
    public virtual Department? Department { get; private set; }
    // --- Bitiş: Yeni Eklenen Alanlar ---

    public Employee(Guid id, string firstName, string lastName, string email, DateTime dateOfBirth, DateTime hireDate)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("First name cannot be empty.", nameof(firstName));
        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("Last name cannot be empty.", nameof(lastName));

        Id = id == Guid.Empty ? Guid.NewGuid() : id;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        DateOfBirth = dateOfBirth;
        HireDate = hireDate;
        Salary = 0;
        DepartmentId = null; // Başlangıçta departmanı yok
    }

    public void UpdatePersonalInfo(string firstName, string lastName, string email, DateTime dateOfBirth)
    {
        if (!string.IsNullOrWhiteSpace(firstName)) FirstName = firstName;
        if (!string.IsNullOrWhiteSpace(lastName)) LastName = lastName;
        if (!string.IsNullOrWhiteSpace(email)) Email = email;
        DateOfBirth = dateOfBirth;
    }

    // --- Metot Güncellemesi ---
    // Departman atama metodu
    public void AssignDepartment(Guid? departmentId)
    {
        // İleride burada domain event'i tetiklenebilir ('EmployeeDepartmentChanged')
        DepartmentId = departmentId;
    }

    // Pozisyon güncelleme metodu (ayrı)
    public void UpdatePosition(string? position)
    {
        Position = position;
    }
    // --- Bitiş: Metot Güncellemesi ---


    public void UpdateSalary(decimal newSalary)
    {
        if (newSalary < 0)
            throw new ArgumentException("Salary cannot be negative.", nameof(newSalary));
        Salary = newSalary;
    }

    // Protected parameterless constructor for EF Core
    protected Employee() { }
}