# Clean Architecture .NET Web API Template

An enterprise-ready Clean Architecture .NET Web API template with comprehensive features for modern web development.

## 🎯 Features

✅ **Clean Architecture** - Domain, Application, Infrastructure, API layers  
✅ **Entity Framework Core** - Data access with SQL Server  
✅ **JWT Authentication** - Secure API endpoints  
✅ **Swagger/OpenAPI** - API documentation  
✅ **Pagination** - Built-in pagination support  
✅ **Unit of Work & Repository Pattern** - Data access patterns  
✅ **Database Seeding** - Initial data setup  
✅ **AutoMapper** - Object-to-object mapping  
✅ **FluentValidation** - Robust input validation  
✅ **CORS Support** - Cross-origin resource sharing  

## 🚀 Enhanced Features

- **AutoMapper Integration**: Automatic mapping between entities and DTOs
- **FluentValidation**: Comprehensive validation with clear error messages
- **Clean Folder Structure**: Organized codebase following best practices
- **Enterprise Patterns**: Unit of Work, Repository, and Service patterns

## 📖 Documentation

For detailed information about the enhanced features, see [FEATURES.md](FEATURES.md).

## 🏃‍♂️ Quick Start

1. Clone the repository
2. Update connection strings in `appsettings.json`
3. Run `dotnet restore`
4. Run `dotnet ef database update`
5. Run `dotnet run`
6. Open `/swagger` for API documentation

## 🏗️ Project Structure

```
src/
├── Template.API/           # Web API layer
├── Template.Application/   # Business logic layer
├── Template.Domain/        # Domain entities
└── Template.Infrastructure/# Data access & external services
```