using HrApp.EmployeeManagement.Domain.Common; // BaseEntity için (varsayılan)
using System.Collections.Generic; // ICollection için

namespace HrApp.EmployeeManagement.Domain.Entities
{
    public class Position : BaseAuditableEntity // BaseEntity veya BaseAuditableEntity hangisi varsa onu kullanın
    {
        // Constructor: Pozisyon adı (Title) zorunlu olsun
        public Position(string title)
        {
            Title = title;
        }

        // Pozisyon Adı/Ünvanı (Örn: Kıdemli Yazılım Geliştirici, İK Uzmanı)
        public string Title { get; private set; } // Sadece içeriden veya constructor ile set edilsin

        // Pozisyon Açıklaması (Opsiyonel)
        public string? Description { get; set; }

        // Bu pozisyona sahip çalışanlar (Navigation Property)
        public virtual ICollection<Employee> Employees { get; private set; } = new HashSet<Employee>();

        // Pozisyon adını güncellemek için public bir metot
        public void UpdateTitle(string newTitle)
        {
            if (!string.IsNullOrWhiteSpace(newTitle) && Title != newTitle)
            {
                Title = newTitle;
                // İleride buraya bir Domain Event tetikleme eklenebilir
            }
        }

        // Bu constructor EF Core için gerekli olabilir veya bazı ORM senaryoları için.
        protected Position() { }
    }
}