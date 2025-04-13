namespace HrApp.EmployeeManagement.Domain.Common
{
    public abstract class BaseEntity
    {
        // Tüm entity'ler için ortak bir ID alanı.
        // Guid kullanmak, özellikle dağıtık sistemlerde (gelecekte microservice olursa)
        // çakışma riskini azaltır ve ID'nin veritabanına eklenmeden önce bilinmesini sağlar.
        // Alternatif olarak int de kullanılabilir.
        public Guid Id { get; protected set; } = Guid.NewGuid();
    }
}