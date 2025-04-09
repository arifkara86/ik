# Dockerfile for HrApp.EmployeeManagement.Api (Updated)

# 1. Build Stage: Uygulamayı derlemek için .NET SDK imajını kullan
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source

# Önce sadece proje dosyalarını kopyala
COPY *.sln .
COPY src/HrApp.EmployeeManagement.Api/*.csproj ./src/HrApp.EmployeeManagement.Api/
COPY src/HrApp.EmployeeManagement.Application/*.csproj ./src/HrApp.EmployeeManagement.Application/
COPY src/HrApp.EmployeeManagement.Domain/*.csproj ./src/HrApp.EmployeeManagement.Domain/
COPY src/HrApp.EmployeeManagement.Infrastructure/*.csproj ./src/HrApp.EmployeeManagement.Infrastructure/

# Bağımlılıkları restore et (Sadece proje dosyaları varken)
RUN dotnet restore HrApp.sln

# Tüm kaynak kodunu kopyala (proje dosyaları hariç - zaten kopyalandı)
# .dockerignore dosyası bin/obj gibi klasörlerin kopyalanmasını engelleyecektir.
COPY . .

# Publish işlemini ayrı bir katmanda yapalım (restore'dan sonra)
WORKDIR /source/src/HrApp.EmployeeManagement.Api
RUN dotnet publish -c Release -o /app/publish --no-restore
# Alternatif: Restore'u publish ile birlikte yapmak:
# RUN dotnet publish -c Release -o /app/publish

# 2. Runtime Stage: Uygulamayı çalıştırmak için ASP.NET Core Runtime imajını kullan
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Publish edilmiş çıktıları build stage'den kopyala
COPY --from=build /app/publish .

# Uygulamanın dinleyeceği portu belirt
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

# Uygulama başlangıç noktası
ENTRYPOINT ["dotnet", "HrApp.EmployeeManagement.Api.dll"]