using HrApp.EmployeeManagement.Domain.Common; // BaseEntity için (varsayılan)
using System.Collections.Generic; // ICollection için

namespace HrApp.EmployeeManagement.Domain.Entities
{
    public class Department : BaseAuditableEntity // BaseEntity veya BaseAuditableEntity hangisi varsa onu kullanın
    {
        // Constructor: Departman adı zorunlu olsun
        public Department(string name)
        {
            Name = name;
        }

        // Departman Adı (Örn: Yazılım Geliştirme, İnsan Kaynakları)
        public string Name { get; private set; } // Sadece içeriden veya constructor ile set edilsin

        // Departman Açıklaması (Opsiyonel)
        public string? Description { get; set; }

        // Bu departmana bağlı çalışanlar (Navigation Property)
        // 'virtual' EF Core'un Lazy Loading yapabilmesi içindir (isteğe bağlı)
        // HashSet kullanmak koleksiyon operasyonları için genellikle daha performanslıdır.
        public virtual ICollection<Employee> Employees { get; private set; } = new HashSet<Employee>();

        // Departman adını güncellemek için public bir metot (İyi pratik)
        public void UpdateName(string newName)
        {
            if (!string.IsNullOrWhiteSpace(newName) && Name != newName)
            {
                Name = newName;
                // İleride buraya bir Domain Event tetikleme eklenebilir
            }
        }

        // Bu constructor EF Core için gerekli olabilir veya bazı ORM senaryoları için.
        // Eğer BaseEntity/BaseAuditableEntity zaten parametresiz constructor içeriyorsa buna gerek kalmayabilir.
        protected Department() { }
    }
}