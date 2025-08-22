# Clean Architecture .NET Web API Template - Enhanced

This is an enterprise-ready Clean Architecture .NET Web API template with enhanced features for validation and object mapping.

## 🏗️ Architecture

The solution follows Clean Architecture principles with the following layers:

- **Domain**: Contains entities, value objects, and domain logic
- **Application**: Contains business logic, DTOs, interfaces, services, and application rules
- **Infrastructure**: Contains data access, external services, and infrastructure concerns
- **API**: Contains controllers, request/response models, and API-specific logic

## 🚀 New Features Added

### AutoMapper Integration
- **Location**: `Application/Common/Mapping/MappingProfile.cs`
- **Purpose**: Automatic mapping between domain entities and DTOs
- **Benefits**: Reduces boilerplate code, maintainable mappings
- **Usage**: Automatically maps between `Patient ↔ PatientDto`, `Doctor ↔ DoctorDto`, etc.

### FluentValidation
- **Location**: `Application/Common/Validators/`
- **Purpose**: Robust validation for DTOs with clear, readable validation rules
- **Features**:
  - Patient validation: Name (2-100 chars), DateOfBirth (realistic dates)
  - Doctor validation: Name (2-100 chars), Specialty (2-100 chars)
- **Integration**: Automatically validates requests in API controllers

### Enhanced Folder Structure
```
src/
├── Template.API/
│   ├── Common/Mapping/          # API-level mappings
│   ├── Controllers/             # API controllers
│   ├── Extensions/              # Service registration extensions
│   └── Models/                  # Request/Response models
├── Template.Application/
│   ├── Common/
│   │   ├── Interfaces/          # All application interfaces (moved here)
│   │   ├── Mapping/             # AutoMapper profiles
│   │   └── Validators/          # FluentValidation validators
│   ├── DTOs/                    # Data Transfer Objects
│   └── Services/                # Application services
├── Template.Domain/
│   └── Entities/                # Domain entities
└── Template.Infrastructure/
    ├── Data/                    # Data access layer
    └── Services/                # Infrastructure services
```

## 🔧 Technology Stack

- **.NET 8**: Latest LTS version
- **AutoMapper 12.0.1**: Object-to-object mapping
- **FluentValidation 11.11.0**: Validation library
- **Entity Framework Core**: Data access
- **JWT Authentication**: Security
- **Swagger/OpenAPI**: API documentation
- **Clean Architecture**: Architectural pattern

## 📋 Usage Examples

### AutoMapper in Services
```csharp
public async Task<PatientDto> CreatePatientAsync(CreatePatientDto createPatientDto)
{
    var patient = _mapper.Map<Patient>(createPatientDto);
    await _unitOfWork.Patients.AddAsync(patient);
    await _unitOfWork.CompleteAsync();
    return _mapper.Map<PatientDto>(patient);
}
```

### FluentValidation Rules
```csharp
public class CreatePatientDtoValidator : AbstractValidator<CreatePatientDto>
{
    public CreatePatientDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters");

        RuleFor(x => x.DateOfBirth)
            .LessThan(DateTime.Now).WithMessage("Date of birth must be in the past");
    }
}
```

## ⚙️ Configuration

### Dependency Injection Registration

**Application Layer** (`Template.Application/DependencyInjection.cs`):
```csharp
services.AddAutoMapper(Assembly.GetExecutingAssembly());
services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
```

**API Layer** (`Template.API/Extensions/ServiceCollectionExtensions.cs`):
```csharp
services.AddFluentValidationAutoValidation();
services.AddFluentValidationClientsideAdapters();
services.AddAutoMapper(Assembly.GetExecutingAssembly());
```

## 🎯 Benefits

1. **Maintainability**: Clear separation of concerns, easy to modify and extend
2. **Testability**: Loosely coupled architecture makes unit testing straightforward
3. **Scalability**: Clean architecture supports growing application complexity
4. **Validation**: Robust validation with clear error messages
5. **Mapping**: Automatic object mapping reduces boilerplate code
6. **Enterprise-Ready**: Follows enterprise patterns and best practices

## 🚦 Getting Started

1. Clone the repository
2. Run `dotnet restore` to restore NuGet packages
3. Update connection strings in `appsettings.json`
4. Run `dotnet ef database update` to create the database
5. Run `dotnet run` to start the API
6. Navigate to `/swagger` to view the API documentation

## 📝 Next Steps

- Add comprehensive unit tests
- Implement integration tests
- Add health checks
- Implement caching strategies
- Add logging with Serilog
- Implement API versioning
- Add rate limiting
- Implement background services with Hangfire
