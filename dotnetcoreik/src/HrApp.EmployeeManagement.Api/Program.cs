// HrApp.EmployeeManagement.Api/Program.cs

using AppValidationException = HrApp.EmployeeManagement.Application.Exceptions.ValidationException; // Alias
using HrApp.EmployeeManagement.Application.Exceptions;
using HrApp.EmployeeManagement.Application.Features.Employees.Commands.CreateEmployee;
using HrApp.EmployeeManagement.Application.Features.Employees.Commands.UpdateEmployee;
using HrApp.EmployeeManagement.Application.Features.Employees.Commands.DeleteEmployee;
using HrApp.EmployeeManagement.Application.Features.Employees.Queries.GetAllEmployees;
using HrApp.EmployeeManagement.Application.Features.Employees.Queries.GetEmployeeById;
using HrApp.EmployeeManagement.Application.Persistence; // Artık Controller'da gerekli değil
using HrApp.EmployeeManagement.Domain.Entities; // Dönüş tipleri için
// using HrApp.EmployeeManagement.Infrastructure.Data; // Artık Controller'da gerekli değil
using HrApp.EmployeeManagement.Infrastructure.Repositories; // Artık Controller'da gerekli değil
using HrApp.EmployeeManagement.Application.Persistence;
using HrApp.EmployeeManagement.Infrastructure.Repositories;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
// using Npgsql; // Artık Controller'da gerekli değil
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using FluentValidation;
using HrApp.EmployeeManagement.Application.Behaviours;
// using HrApp.EmployeeManagement.Infrastructure.Data; // DbContext'i Migrate için almak gerekebilir
// using Microsoft.Extensions.Logging; // Migrate loglaması için

var builder = WebApplication.CreateBuilder(args);

// Veritabanı Bağlantı Cümlesi
var connectionString = builder.Configuration.GetConnectionString("EmployeeManagementDb");
if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Connection string 'EmployeeManagementDb' not found.");
}


// === Servisleri Konteynere Ekle ===

// DbContext Eklenmesi
builder.Services.AddDbContext<HrApp.EmployeeManagement.Infrastructure.Data.EmployeeManagementDbContext>(options =>
    options.UseNpgsql(connectionString));

// Repository Eklenmesi
builder.Services.AddScoped<HrApp.EmployeeManagement.Application.Persistence.IEmployeeRepository, HrApp.EmployeeManagement.Infrastructure.Repositories.EmployeeRepository>();
builder.Services.AddScoped<HrApp.EmployeeManagement.Application.Persistence.IDepartmentRepository, HrApp.EmployeeManagement.Infrastructure.Repositories.DepartmentRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
// FluentValidation Validator'larını Ekle
builder.Services.AddValidatorsFromAssembly(Assembly.Load("HrApp.EmployeeManagement.Application"));

// AutoMapper'ı Ekle
builder.Services.AddAutoMapper(Assembly.Load("HrApp.EmployeeManagement.Application"));

// MediatR'ı Ekle
builder.Services.AddMediatR(cfg => {
    cfg.RegisterServicesFromAssembly(Assembly.Load("HrApp.EmployeeManagement.Application"));
    cfg.AddOpenBehavior(typeof(ValidationBehaviour<,>));
});

// --- DEĞİŞİKLİK: CORS Politikası Ekleme ---
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowWebApp", // Politikaya bir isim veriyoruz
        policyBuilder =>
        {
            policyBuilder
                .WithOrigins("http://localhost:5173") // Frontend'in çalıştığı adrese izin ver
                // .AllowAnyOrigin() // VEYA tüm kaynaklara izin ver (geliştirme için olabilir ama canlıda güvensiz)
                .AllowAnyHeader() // Tüm header'lara izin ver
                .AllowAnyMethod(); // Tüm HTTP metotlarına (GET, POST, PUT, DELETE vb.) izin ver
                // .AllowCredentials(); // Eğer kimlik bilgisi (cookie vb.) gönderilecekse gerekir
        });
});
// --- CORS EKLEME SONU ---


// API Controller'larını Ekle
builder.Services.AddControllers();

// Swagger/OpenAPI Eklemeleri
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "HrApp Employee Management API", Version = "v1" });
});

// === Uygulama Yapılandırması ===
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "HrApp API v1"));
    ApplyMigrations(app);
}

// app.UseHttpsRedirection();

// --- DEĞİŞİKLİK: CORS Middleware'ini Ekle ---
// ÖNEMLİ: UseCors, UseRouting'den SONRA, UseAuthorization ve MapControllers'dan ÖNCE çağrılmalıdır.
app.UseRouting(); // Bunu eklemek iyi bir pratik olabilir (varsayılan olarak ekli olabilir)

app.UseCors("AllowWebApp"); // Tanımladığımız politikayı etkinleştir

app.UseAuthorization();
// --- CORS EKLEME SONU ---


app.MapControllers();

app.Run();// Uygulamayı çalıştır


// === Migration Uygulama Metodu ===
static void ApplyMigrations(IApplicationBuilder app)
{
     using (var scope = app.ApplicationServices.CreateScope())
    {
        var services = scope.ServiceProvider;
        try
        {
            // DbContext'i Infrastructure namespace'i ile belirtelim
            var context = services.GetRequiredService<HrApp.EmployeeManagement.Infrastructure.Data.EmployeeManagementDbContext>();
            // ILogger'ı Infrastructure namespace'i ile belirtelim (veya using ekleyelim)
            var logger = services.GetRequiredService<ILogger<Program>>(); // Program yerine daha uygun bir tip

            logger.LogInformation("Attempting to apply database migrations...");
            context.Database.Migrate(); // EF Core migration'larını uygula
            logger.LogInformation("Database migrations applied successfully or database already up-to-date.");

            // SeedData.Initialize(services); // Başlangıç verisi ekleme (isteğe bağlı)
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred while migrating or initializing the database.");
            // Geliştirme sırasında hatayı görmek için fırlatmak iyi olabilir.
            // throw;
        }
    }
}