# Enterprise-Scale Clean Architecture Structure

For applications with 10-20+ entities, here's the recommended **Feature-Based Architecture**:

## 🏗️ **Proposed Structure**

```
src/
├── Template.API/
│   ├── Controllers/
│   │   ├── Common/
│   │   │   └── BaseController.cs
│   │   │── V1/
│   │   │   ├── AuthController.cs
│   │   │   ├── DoctorsController.cs
│   │   │   └── PatientsController.cs
│   │   ├── V2/
│   │   │   └── (new controllers for V2)
│   │   ├── ErrorController.cs   
│   ├── Models/
│   │   ├── Auth/
│   │   │   ├── LoginRequest.cs
│   │   │   ├── RegisterRequest.cs
│   │   │   ├── AuthResponse.cs
│   │   │   └── RefreshTokenRequest.cs
│   │   ├── Doctors/
│   │   │   ├── CreateDoctorRequest.cs
│   │   │   ├── UpdateDoctorRequest.cs
│   │   │   ├── DoctorResponse.cs
│   │   │   └── DoctorListResponse.cs
│   │   ├── Patients/
│   │   │   ├── CreatePatientRequest.cs
│   │   │   ├── UpdatePatientRequest.cs
│   │   │   ├── PatientResponse.cs
│   │   │   └── PatientListResponse.cs
│   │   └── Common/
│   │       ├── ApiResponse.cs
│   │       └── PaginatedResponse.cs
│   ├── Common/
│   │   ├── Attributes/
│   │   │   └── CacheAttribute.cs
│   │   ├── Extensions/
│   │   │   └── ModelStateExtensions.cs
│   │   ├── Filters/
│   │   │   └── ExceptionFilter.cs
│   │   ├── Mapping/
│   │   │   ├── DoctorMappingProfile.cs
│   │   │   ├── PatientMappingProfile.cs
│   │   │   └── BaseMappingProfile.cs 
│   │   └── Models/
│   │       └── (Common model definitions)
│   ├── Extensions/
│   │   ├── ServiceCollectionExtensions.cs
│   │   ├── DatabaseSeederExtensions.cs
│   │   └── SwaggerExtensions.cs
│   ├── Middleware/
│   │   └── (Custom middleware components)
│   ├── Services/
│   │   └── ValidationService.cs
│   ├── Properties/
│   │   └── launchSettings.json
│   ├── Program.cs
│   ├── Template.API.csproj
│   ├── appsettings.json
│
├── Template.Application/
│   ├── Features/
│   │   ├── Doctors/
│   │   │   ├── Commands/
│   │   │   │   ├── CreateDoctorCommand.cs
│   │   │   │   ├── UpdateDoctorCommand.cs
│   │   │   │   ├── DeleteDoctorCommand.cs
│   │   │   ├── DTOs/
│   │   │   │   ├── DoctorDto.cs
│   │   │   │   ├── CreateDoctorDto.cs
│   │   │   │   ├── UpdateDoctorDto.cs
│   │   │   ├── Queries/
│   │   │   │   ├── GetDoctorQuery.cs
│   │   │   │   ├── GetDoctorsQuery.cs
│   │   │   │   ├── GetDoctorsBySpecialtyQuery.cs
│   │   │   ├── Validators/
│   │   │   │   ├── DoctorValidator.cs
│   │   │   │   ├── CreateDoctorValidator.cs
│   │   │   │   ├── UpdateDoctorValidator.cs
│   │   │   │   ├── DeleteDoctorValidator.cs
│   │   ├── Patients/
│   │   │   ├── Commands/
│   │   │   ├── DTOs/
│   │   │   ├── Queries/
│   │   │   └── Validators/
│   │   └── ... (one per entity/feature)
│   ├── Common/
│   │   ├── Behaviors/
│   │   │   ├── ValidationBehavior.cs
│   │   │   ├── LoggingBehavior.cs
│   │   ├── Commands/
│   │   │   ├── BaseCommand.cs
│   │   │   ├── ICommand.cs
│   │   ├── Exceptions/
│   │   │   ├── ForbiddenException.cs
│   │   │   ├── NotFoundException.cs
│   │   │   ├── ValidationException.cs
│   │   ├── Interfaces/
│   │   │   ├── ICurrentUserService.cs
│   │   │   ├── IDateTimeService.cs
│   │   │   ├── IRepository.cs
│   │   │   ├── ITokenService.cs
│   │   │   ├── IUnitOfWork.cs
│   │   ├── Mapping/
│   │   │   ├── DoctorMappingProfile.cs
│   │   │   ├── PatientMappingProfile.cs
│   │   │   └── (other AutoMapper profiles)
│   │   ├── Models/
│   │   │   ├── PaginatedRequest.cs
│   │   │   ├── PaginatedResult.cs
│   │   │   ├── Result.cs
│   │   ├── Queries/
│   │   │   ├── BaseQuery.cs
│   │   │   ├── IQuery.cs
│   │   ├── Services/
│   │   │   ├── NotificationService.cs
│   │   │   ├── AuthService.cs
│   │   └── Settings/
│   │       ├── JwtSettings.cs           # JWT business rules
│   │       ├── EmailSettings.cs         # Email business rules
│   │       ├── CacheSettings.cs         # Cache business policies
│   │       ├── DefaultUsersAndRolesOptions.cs  # Default users and roles configuration
│   │       └── UserSeedOptions.cs       # User seed configuration options
│   └── DependencyInjection.cs
│
├── Template.Infrastructure/
│   ├── Configuration/
│   │   ├── DatabaseConfiguration.cs    # EF Core setup
│   │   ├── EmailConfiguration.cs       # SMTP/SendGrid setup
│   │   ├── IdentityConfiguration.cs    # Identity setup
│   │   ├── RepositoryConfiguration.cs  # Repository setup
│   │   ├── SeedConfiguration.cs        # Database seeding setup
│   │   └── CacheConfiguration.cs       # Redis/Memory cache setup
│   ├── Data/
│   │   ├── Contexts/
│   │   │   ├── AppDbContext.cs
│   │   │   └── IdentityDbContext.cs
│   │   ├── Repositories/
│   │   │   ├── GenericRepository.cs
│   │   │   ├── DoctorRepository.cs
│   │   │   └── PatientRepository.cs
│   │   ├── UnitOfWork.cs
│   │   └── Seed/
│   │       └── DbSeeder.cs
│   ├── Services/
│   │   ├── CurrentUserService.cs
│   │   ├── DateTimeService.cs
│   │   └── TokenService.cs
│   ├── Migrations/
│   │   ├── 20250819195941_InitialCreate.cs
│   │   ├── 20250819195941_InitialCreate.Designer.cs
│   │   ├── IdentityDbContextModelSnapshot.cs
│   │   └── AppDb/
│   │       ├── 20250822070251_CreateAppDbSchema.cs
│   │       ├── 20250822070251_CreateAppDbSchema.Designer.cs
│   │       └── AppDbContextModelSnapshot.cs
│   ├── DependencyInjection.cs
│   └── Template.Infrastructure.csproj
│
└── Template.Domain/
    ├── Common/
    │   ├── BaseEntity.cs                # Enhanced base with common properties
    ├── Entities/
    │   ├── Doctor.cs                    # Doctor domain entity
    │   ├── Patient.cs                   # Patient domain entity
    │   ├── UserProfile.cs               # User profile entity
    │   └── ... (future entities like Appointment.cs, Prescription.cs, etc.)
    ├── Identity/
    │   ├── ApplicationUser.cs           # Identity user entity
    │   └── ... (future identity-related entities)
    ├── ValueObjects/
    │   ├── Address.cs                   # Address value object (future)
    │   ├── PhoneNumber.cs               # Phone number value object (future)
    │   ├── Email.cs                     # Email value object (future)
    │   └── ... (other value objects as needed)
    ├── Enums/
    │   ├── DoctorSpecialty.cs           # Doctor specialties enumeration
    │   ├── PatientStatus.cs             # Patient status enumeration
    │   └── ... (other enumerations)
    ├── Events/
    │   ├── DoctorEvents/
    │   │   ├── DoctorCreatedEvent.cs    # Domain event for doctor creation
    │   │   └── DoctorUpdatedEvent.cs    # Domain event for doctor updates
    │   ├── PatientEvents/
    │   │   ├── PatientCreatedEvent.cs   # Domain event for patient creation
    │   │   └── PatientUpdatedEvent.cs   # Domain event for patient updates
    │   └── ... (events for other entities)
    ├── Exceptions/
    │   ├── DoctorNotAvailableException.cs   # Doctor-specific exceptions
    │   └── DomainException.cs           # Base domain exception
    └── Template.Domain.csproj
```

## 🎯 **Key Benefits of This Structure**

### 1. **Feature-Based Organization**
- Each entity/feature is self-contained
- Easy to locate and modify feature-specific code
- Clear boundaries between different business capabilities

### 2. **Scalability** 
- Adding new entities doesn't clutter existing folders
- Each feature can evolve independently
- Easy to assign different teams to different features

### 3. **CQRS Pattern Ready**
- Separate Commands and Queries folders
- Clear separation of read vs write operations
- Each operation has its own handler and validator

### 4. **Maintainability**
- Related files are grouped together
- Easy to understand feature boundaries
- Reduces cognitive load when working on specific features

## 🔧 **Implementation Strategy**

### Phase 1: Reorganize Existing Code
1. Move existing files to feature-based structure
2. Update namespaces and references
3. Test that everything still works

### Phase 2: Enhance with CQRS (Optional)
1. Split services into Command/Query handlers
2. Implement MediatR for better decoupling
3. Add behavior pipelines for cross-cutting concerns

### Phase 3: Add New Features
1. Use the established pattern for new entities
2. Copy folder structure from existing features
3. Maintain consistency across all features

## 📝 **Example File Organization**

**For a "Doctor" feature:**

```
Features/Doctors/
├── Commands/
│   ├── CreateDoctor/
│   │   ├── CreateDoctorCommand.cs      # Command model
│   │   ├── CreateDoctorHandler.cs      # Business logic
│   │   └── CreateDoctorValidator.cs    # Validation rules
│   └── UpdateDoctor/
│       ├── UpdateDoctorCommand.cs
│       ├── UpdateDoctorHandler.cs
│       └── UpdateDoctorValidator.cs
├── Queries/
│   ├── GetDoctor/
│   │   ├── GetDoctorQuery.cs
│   │   └── GetDoctorHandler.cs
│   └── GetDoctors/
│       ├── GetDoctorsQuery.cs
│       └── GetDoctorsHandler.cs
├── DTOs/
│   ├── DoctorDto.cs
│   ├── CreateDoctorDto.cs
│   └── UpdateDoctorDto.cs
└── Services/
    ├── IDoctorService.cs
    └── DoctorService.cs
```

## 🚀 **Migration Benefits**

1. **Developer Experience**: Easy to find and work with related code
2. **Team Collaboration**: Multiple teams can work on different features
3. **Code Reviews**: Smaller, focused pull requests
4. **Testing**: Feature-specific test organization
5. **Documentation**: Self-documenting folder structure

This structure scales excellently for large enterprise applications with many entities and complex business logic.
