# Delivery Platform Project - Key Questions

## Business & Market
- What type of delivery service will this be? (food, groceries, packages, etc.)
- Who is the target audience?
- What's the competitive landscape?
- What's the unique value proposition?
- What's the expected market size and growth potential?

## Technical Requirements
- What platforms need to be supported? (web, mobile, both)
- What's the expected scale? (users, orders per day/hour)
- What are the core features needed for MVP?
- What third-party integrations are required? (payment, maps, notifications)
- What's the technology stack preference?

## Operations & Logistics
- Will this handle the delivery fleet or integrate with existing services?
- What's the delivery radius and coverage area?
- How will order tracking and real-time updates work?
- What's the commission/pricing model?
- How will customer support be handled?

## Resources & Timeline
- What's the available budget?
- What's the team size and skill composition?
- What's the target launch timeline?
- What are the key milestones and deliverables?
- What's the go-to-market strategy?

## Legal & Compliance
- What are the regulatory requirements in target markets?
- What licenses or permits are needed?
- What are the data privacy and security requirements?
- What insurance and liability considerations exist?

## System Architecture & Extensibility

### Core Platform Design
- **Service-Agnostic Foundation**: Design a modular platform that abstracts common functionality (user management, payments, real-time tracking, notifications) from service-specific logic
- **Plugin Architecture**: Implement a plugin system where different service types (delivery, taxi, rideshare) can be added as modules
- **Shared Components**: 
  - User authentication and profiles
  - Payment processing
  - Geolocation and mapping services
  - Real-time communication layer
  - Analytics and reporting
  - Admin dashboard framework

### Service-Specific Modules
- **Delivery Module**: Order management, restaurant/store integration, delivery routing
- **Taxi Module**: Ride booking, driver dispatch, fare calculation
- **Future Modules**: Package delivery, grocery shopping, ride-sharing, etc.

### Technical Architecture Considerations
- **Microservices Architecture**: Separate services for user management, payments, notifications, geolocation, and business logic
- **Event-Driven Design**: Use message queues and event streaming for loose coupling between services
- **API Gateway**: Centralized routing and authentication for all service modules
- **Database Strategy**: Shared core tables with service-specific extensions
- **Configuration Management**: Feature flags and service-specific configurations
- **Scalability**: Horizontal scaling capabilities for each service module independently

### Technology Stack

#### Backend
- **.NET 8 / C#**: Modern, cross-platform framework for building scalable APIs
- **Entity Framework Core**: ORM for database operations with code-first migrations
- **ASP.NET Core Web API**: RESTful API development with built-in dependency injection
- **SignalR**: Real-time communication for live tracking and notifications
- **AutoMapper**: Object-to-object mapping for clean data transfer
- **FluentValidation**: Input validation with fluent interface
- **Serilog**: Structured logging for monitoring and debugging

#### Frontend
- **Next.js 14**: React framework with server-side rendering and app router
- **React 18**: Component-based UI library with hooks and concurrent features
- **TypeScript**: Type-safe development for better code quality and maintainability
- **Tailwind CSS**: Utility-first CSS framework for responsive design
- **React Query**: Data fetching and caching for efficient API communication
- **React Hook Form**: Form handling with validation
- **Zustand**: Lightweight state management

#### Database & Infrastructure
- **PostgreSQL**: Primary database for transactional data
- **Redis**: Caching and session management
- **Docker**: Containerization for consistent deployment
- **Azure/AWS**: Cloud hosting and services

## Identity Service (Authentication & Authorization)

### Service Structure
```
backend/src/Services/IdentityService/
├── IdentityService.API/
│   ├── Controllers/
│   │   ├── AuthController.cs
│   │   ├── AccountController.cs
│   │   ├── TokenController.cs
│   │   └── ExternalAuthController.cs
│   ├── Middleware/
│   │   ├── JwtMiddleware.cs
│   │   └── RateLimitingMiddleware.cs
│   ├── Program.cs
│   └── appsettings.json
├── IdentityService.Application/
│   ├── Commands/
│   │   ├── RegisterUserCommand.cs
│   │   ├── LoginCommand.cs
│   │   ├── RefreshTokenCommand.cs
│   │   ├── ResetPasswordCommand.cs
│   │   ├── ChangePasswordCommand.cs
│   │   └── VerifyEmailCommand.cs
│   ├── Queries/
│   │   ├── GetUserByIdQuery.cs
│   │   └── ValidateTokenQuery.cs
│   ├── Handlers/
│   ├── DTOs/
│   │   ├── LoginRequest.cs
│   │   ├── RegisterRequest.cs
│   │   ├── TokenResponse.cs
│   │   └── UserDto.cs
│   ├── Validators/
│   └── Interfaces/
│       ├── ITokenService.cs
│       ├── IAuthService.cs
│       └── IEmailService.cs
├── IdentityService.Domain/
│   ├── Entities/
│   │   ├── ApplicationUser.cs
│   │   ├── ApplicationRole.cs
│   │   ├── RefreshToken.cs
│   │   └── UserClaim.cs
│   ├── Enums/
│   │   ├── UserType.cs
│   │   └── AuthProvider.cs
│   └── Events/
│       ├── UserRegisteredEvent.cs
│       └── PasswordChangedEvent.cs
└── IdentityService.Infrastructure/
    ├── Data/
    │   ├── IdentityDbContext.cs
    │   └── Migrations/
    ├── Services/
    │   ├── TokenService.cs
    │   ├── AuthService.cs
    │   └── EmailService.cs
    ├── Repositories/
    └── Configuration/
        ├── JwtSettings.cs
        └── OpenIddictConfiguration.cs
```

### Core Features

#### Authentication
- **JWT Token Authentication**: Access and refresh token management
- **OAuth 2.0 / OpenID Connect**: Standards-compliant authentication via OpenIddict
- **Multi-Factor Authentication (MFA)**: TOTP-based 2FA support
- **External Providers**: Google, Apple, Facebook sign-in integration
- **Password Management**: Secure hashing with bcrypt, password reset flows

#### Authorization
- **Role-Based Access Control (RBAC)**: Customer, Partner, Driver, Admin roles
- **Claims-Based Authorization**: Fine-grained permissions
- **Policy-Based Authorization**: Custom authorization policies
- **Resource-Based Authorization**: Owner-only access patterns

#### User Management
- **Registration**: Email/phone verification workflows
- **Profile Management**: Update personal information
- **Account Recovery**: Password reset via email/SMS
- **Session Management**: Device tracking and logout capabilities
- **Account Deactivation**: Soft delete with data retention policies

#### Security Features
- **Rate Limiting**: Brute force protection on auth endpoints
- **Token Revocation**: Immediate logout and token invalidation
- **Audit Logging**: Track authentication events and changes
- **IP Whitelisting**: Admin access restrictions
- **CORS Configuration**: Cross-origin request security

### Key Endpoints
| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/api/auth/register` | User registration |
| POST | `/api/auth/login` | User login |
| POST | `/api/auth/refresh` | Refresh access token |
| POST | `/api/auth/logout` | Revoke refresh token |
| POST | `/api/auth/forgot-password` | Initiate password reset |
| POST | `/api/auth/reset-password` | Complete password reset |
| POST | `/api/auth/verify-email` | Email verification |
| POST | `/api/auth/external/{provider}` | External OAuth login |
| GET | `/api/account/profile` | Get user profile |
| PUT | `/api/account/profile` | Update user profile |
| POST | `/api/account/change-password` | Change password |
| POST | `/api/account/enable-2fa` | Enable MFA |

backend/src/Services/UserService/
├── UserService.API/
│   ├── Controllers/
│   │   ├── UsersController.cs
│   │   ├── CustomersController.cs
│   │   ├── PartnersController.cs
│   │   ├── DriversController.cs
│   │   └── AdminUsersController.cs
│   ├── Middleware/
│   │   └── UserContextMiddleware.cs
│   ├── Program.cs
│   └── appsettings.json
├── UserService.Application/
│   ├── Commands/
│   │   ├── CreateUserCommand.cs
│   │   ├── UpdateUserCommand.cs
│   │   ├── DeleteUserCommand.cs
│   │   ├── CreateCustomerProfileCommand.cs
│   │   ├── CreatePartnerProfileCommand.cs
│   │   ├── CreateDriverProfileCommand.cs
│   │   ├── UpdateAddressCommand.cs
│   │   └── AdminCreateUserCommand.cs
│   ├── Queries/
│   │   ├── GetUserByIdQuery.cs
│   │   ├── GetUserByEmailQuery.cs
│   │   ├── GetUsersListQuery.cs
│   │   ├── GetCustomerProfileQuery.cs
│   │   └── SearchUsersQuery.cs
│   ├── Handlers/
│   │   ├── CreateUserCommandHandler.cs
│   │   ├── UpdateUserCommandHandler.cs
│   │   └── GetUserByIdQueryHandler.cs
│   ├── DTOs/
│   │   ├── UserDto.cs
│   │   ├── CustomerProfileDto.cs
│   │   ├── PartnerProfileDto.cs
│   │   ├── DriverProfileDto.cs
│   │   ├── AddressDto.cs
│   │   └── CreateUserFromIdentityDto.cs
│   ├── Validators/
│   │   ├── CreateUserValidator.cs
│   │   └── UpdateUserValidator.cs
│   ├── Interfaces/
│   │   ├── IUserRepository.cs
│   │   └── IUserService.cs
│   └── EventHandlers/
│       ├── UserRegisteredEventHandler.cs
│       └── UserCreatedByAdminEventHandler.cs
├── UserService.Domain/
│   ├── Entities/
│   │   ├── User.cs
│   │   ├── CustomerProfile.cs
│   │   ├── PartnerProfile.cs
│   │   ├── DriverProfile.cs
│   │   ├── Address.cs
│   │   └── UserPreferences.cs
│   ├── Enums/
│   │   ├── UserStatus.cs
│   │   ├── UserType.cs
│   │   └── VerificationStatus.cs
│   ├── ValueObjects/
│   │   ├── PhoneNumber.cs
│   │   └── Email.cs
│   └── Events/
│       ├── UserCreatedEvent.cs
│       ├── UserUpdatedEvent.cs
│       └── ProfileCompletedEvent.cs
└── UserService.Infrastructure/
    ├── Data/
    │   ├── UserDbContext.cs
    │   └── Migrations/
    ├── Repositories/
    │   ├── UserRepository.cs
    │   └── AddressRepository.cs
    ├── Services/
    │   └── UserService.cs
    └── Consumers/
        ├── UserRegisteredConsumer.cs
        └── AdminUserCreatedConsumer.cs
```

### Inter-Service Communication Flow

#### Flow 1: Self-Registration (Customer/Partner)
```
┌─────────────┐    ┌──────────────────┐    ┌─────────────────┐    ┌─────────────┐
│   Client    │───▶│ Identity Service │───▶│  Message Queue  │───▶│ User Service│
│  (Web/App)  │    │                  │    │   (RabbitMQ)    │    │             │
└─────────────┘    └──────────────────┘    └─────────────────┘    └─────────────┘
      │                    │                        │                    │
      │  1. POST /register │                        │                    │
      │───────────────────▶│                        │                    │
      │                    │                        │                    │
      │                    │  2. Create Auth User   │                    │
      │                    │  (credentials only)    │                    │
      │                    │                        │                    │
      │                    │  3. Publish Event      │                    │
      │                    │─────────────────────▶ │                    │
      │                    │   UserRegisteredEvent  │                    │
      │                    │                        │                    │
      │                    │                        │  4. Consume Event  │
      │                    │                        │───────────────────▶│
      │                    │                        │                    │
      │                    │                        │  5. Create User    │
      │                    │                        │     Profile        │
      │                    │                        │                    │
      │  6. Return JWT     │                        │                    │
      │◀───────────────────│                        │                    │
```

#### Flow 2: Admin Creates User
```
┌───────────────┐    ┌─────────────┐    ┌──────────────────┐    ┌─────────────────┐
│ Admin Dashboard│───▶│ User Service│───▶│ Identity Service │───▶│  Message Queue  │
│               │    │             │    │   (via gRPC)     │    │                 │
└───────────────┘    └─────────────┘    └──────────────────┘    └─────────────────┘
      │                    │                    │                        │
      │ 1. POST /admin/users│                   │                        │
      │───────────────────▶│                    │                        │
      │                    │                    │                        │
      │                    │ 2. Validate &      │                        │
      │                    │    Create Profile  │                        │
      │                    │                    │                        │
      │                    │ 3. gRPC: Create    │                        │
      │                    │    Auth Credentials│                        │
      │                    │───────────────────▶│                        │
      │                    │                    │                        │
      │                    │                    │ 4. Create Auth User   │
      │                    │                    │    with temp password │
      │                    │                    │                        │
      │                    │ 5. Return Auth ID  │                        │
      │                    │◀───────────────────│                        │
      │                    │                    │                        │
      │                    │                    │ 6. Publish Event       │
      │                    │                    │────────────────────────▶
      │                    │                    │  AdminUserCreatedEvent │
      │                    │                    │  (triggers welcome email)
      │ 7. Return User     │                    │                        │
      │◀───────────────────│                    │                        │
```

### Event Contracts

```csharp
// Published by IdentityService after self-registration
public record UserRegisteredEvent
{
    public Guid IdentityUserId { get; init; }
    public string Email { get; init; }
    public string? PhoneNumber { get; init; }
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public UserType UserType { get; init; }
    public AuthProvider AuthProvider { get; init; }
    public DateTime RegisteredAt { get; init; }
}

// Published by IdentityService after admin creates user
public record AdminUserCreatedEvent
{
    public Guid IdentityUserId { get; init; }
    public Guid UserProfileId { get; init; }
    public string Email { get; init; }
    public string TemporaryPassword { get; init; }
    public Guid CreatedByAdminId { get; init; }
    public DateTime CreatedAt { get; init; }
}

// Published by UserService after profile completion
public record UserProfileCompletedEvent
{
    public Guid UserId { get; init; }
    public Guid IdentityUserId { get; init; }
    public UserType UserType { get; init; }
    public DateTime CompletedAt { get; init; }
}
```

### Key Endpoints

| Method | Endpoint | Description | Access |
|--------|----------|-------------|--------|
| GET | `/api/users/{id}` | Get user by ID | Authenticated |
| PUT | `/api/users/{id}` | Update user profile | Owner/Admin |
| DELETE | `/api/users/{id}` | Soft delete user | Admin |
| GET | `/api/users` | List users (paginated) | Admin |
| GET | `/api/users/search` | Search users | Admin |
| POST | `/api/admin/users` | Create user (admin) | Admin |
| POST | `/api/admin/users/{id}/activate` | Activate user | Admin |
| POST | `/api/admin/users/{id}/suspend` | Suspend user | Admin |
| GET | `/api/customers/profile` | Get customer profile | Customer |
| PUT | `/api/customers/profile` | Update customer profile | Customer |
| POST | `/api/customers/addresses` | Add address | Customer |
| GET | `/api/partners/profile` | Get partner profile | Partner |
| PUT | `/api/partners/profile` | Update partner profile | Partner |
| GET | `/api/drivers/profile` | Get driver profile | Driver |
| PUT | `/api/drivers/profile` | Update driver profile | Driver |

### gRPC Service Contract (Identity ↔ User)

```protobuf
// identity_user.proto
service IdentityUserService {
    // Called by UserService to create auth credentials
    rpc CreateAuthUser(CreateAuthUserRequest) returns (CreateAuthUserResponse);
    
    // Called by UserService to update auth status
    rpc UpdateAuthStatus(UpdateAuthStatusRequest) returns (UpdateAuthStatusResponse);
    
    // Called by UserService to delete auth user
    rpc DeleteAuthUser(DeleteAuthUserRequest) returns (DeleteAuthUserResponse);
}

message CreateAuthUserRequest {
    string email = 1;
    string phone_number = 2;
    string temporary_password = 3;
    string user_type = 4;
    bool require_password_change = 5;
    string created_by_admin_id = 6;
}

message CreateAuthUserResponse {
    string identity_user_id = 1;
    bool success = 2;
    string error_message = 3;
}
```

### Database Schema Separation

```
┌─────────────────────────────────────┐    ┌─────────────────────────────────────┐
│       Identity Database             │    │         User Database               │
├─────────────────────────────────────┤    ├─────────────────────────────────────┤
│ AspNetUsers                         │    │ Users                               │
│ ├── Id (PK)                         │◄──▶│ ├── Id (PK)                         │
│ ├── Email                           │    │ ├── IdentityUserId (FK reference)   │
│ ├── PasswordHash                    │    │ ├── Email                           │
│ ├── PhoneNumber                     │    │ ├── FirstName                       │
│ ├── TwoFactorEnabled                │    │ ├── LastName                        │
│ └── ...auth fields                  │    │ ├── PhoneNumber                     │
│                                     │    │ ├── UserType                        │
│ RefreshTokens                       │    │ ├── Status                          │
│ ├── Id                              │    │ └── CreatedAt                       │
│ ├── UserId                          │    │                                     │
│ ├── Token                           │    │ CustomerProfiles                    │
│ └── ExpiresAt                       │    │ ├── UserId (FK)                     │
│                                     │    │ ├── PreferredPaymentMethod          │
│ ExternalLogins                      │    │ └── LoyaltyPoints                   │
│ ├── UserId                          │    │                                     │
│ ├── Provider                        │    │ Addresses                           │
│ └── ProviderKey                     │    │ ├── UserId (FK)                     │
│                                     │    │ ├── Type (Home/Work/Other)          │
│                                     │    │ ├── Street, City, etc.              │
│                                     │    │ └── IsDefault                       │
│                                     │    │                                     │
│                                     │    │ PartnerProfiles                     │
│                                     │    │ ├── UserId (FK)                     │
│                                     │    │ ├── BusinessName                    │
│                                     │    │ └── VerificationStatus              │
│                                     │    │                                     │
│                                     │    │ DriverProfiles                      │
│                                     │    │ ├── UserId (FK)                     │
│                                     │    │ ├── LicenseNumber                   │
│                                     │    │ └── VehicleInfo                     │
└─────────────────────────────────────┘    └─────────────────────────────────────┘

### User Type Inheritance Model

```
┌─────────────────────────────────────────────────────────────────────────────────┐
│                              User Database                                       │
├─────────────────────────────────────────────────────────────────────────────────┤
│                                                                                  │
│  ┌─────────────────────────────────────┐                                        │
│  │              Users                  │                                        │
│  │  (Base Table - All User Types)      │                                        │
│  ├─────────────────────────────────────┤                                        │
│  │ Id (PK, GUID)                       │                                        │
│  │ IdentityUserId (Unique, Not Null)   │──────▶ Identity.AspNetUsers.Id         │
│  │ Email (Unique)                      │                                        │
│  │ FirstName                           │                                        │
│  │ LastName                            │                                        │
│  │ PhoneNumber                         │                                        │
│  │ UserType (Enum: Customer/Partner/   │                                        │
│  │           Driver/Admin)             │                                        │
│  │ Status (Active/Suspended/Deleted)   │                                        │
│  │ AvatarUrl                           │                                        │
│  │ CreatedAt                           │                                        │
│  │ UpdatedAt                           │                                        │
│  └──────────────┬──────────────────────┘                                        │
│                 │                                                                │
│                 │ One-to-One (based on UserType)                                 │
│                 │                                                                │
│    ┌────────────┼────────────┬────────────────────┐                             │
│    │            │            │                    │                             │
│    ▼            ▼            ▼                    ▼                             │
│  ┌─────────┐ ┌─────────┐ ┌─────────┐      ┌─────────────┐                       │
│  │Customers│ │Partners │ │ Drivers │      │ AdminUsers  │                       │
│  └────┬────┘ └────┬────┘ └────┬────┘      └──────┬──────┘                       │
│       │           │           │                  │                              │
│       ▼           ▼           ▼                  ▼                              │
│  ┌─────────────────────────────────────────────────────────────────────┐        │
│  │                    Type-Specific Profile Tables                      │        │
│  ├─────────────┬─────────────┬─────────────────┬───────────────────────┤        │
│  │  Customers  │  Partners   │     Drivers     │     AdminUsers        │        │
│  ├─────────────┼─────────────┼─────────────────┼───────────────────────┤        │
│  │ Id (PK)     │ Id (PK)     │ Id (PK)         │ Id (PK)               │        │
│  │ UserId (FK) │ UserId (FK) │ UserId (FK)     │ UserId (FK)           │        │
│  │ Preferred   │ BusinessName│ LicenseNumber   │ Department            │        │
│  │ PaymentId   │ BusinessType│ LicenseExpiry   │ Role                  │        │
│  │ Loyalty     │ TaxId       │ VehicleType     │ Permissions (JSON)    │        │
│  │ Points      │ Commission  │ VehiclePlate    │ LastLoginAt           │        │
│  │ Preferences │ Rate        │ VehicleModel    │ IsSuperAdmin          │        │
│  │ (JSON)      │ Verification│ VehicleYear     │                       │        │
│  │             │ Status      │ InsuranceNumber │                       │        │
│  │             │ Documents   │ InsuranceExpiry │                       │        │
│  │             │ (JSON)      │ BackgroundCheck │                       │        │
│  │             │ BankAccount │ Status          │                       │        │
│  │             │ Info        │ CurrentLocation │                       │        │
│  │             │             │ IsOnline        │                       │        │
│  │             │             │ Rating          │                       │        │
│  └─────────────┴─────────────┴─────────────────┴───────────────────────┘        │
│                                                                                  │
│  ┌─────────────────────────────────────┐                                        │
│  │           Addresses                 │                                        │
│  │  (Shared - Multiple per User)       │                                        │
│  ├─────────────────────────────────────┤                                        │
│  │ Id (PK)                             │                                        │
│  │ UserId (FK) ────────────────────────┼──────▶ Users.Id                        │
│  │ Type (Home/Work/Business/Other)     │                                        │
│  │ Label                               │                                        │
│  │ Street                              │                                        │
│  │ City                                │                                        │
│  │ State                               │                                        │
│  │ PostalCode                          │                                        │
│  │ Country                             │                                        │
│  │ Latitude                            │                                        │
│  │ Longitude                           │                                        │
│  │ IsDefault                           │                                        │
│  │ Instructions                        │                                        │
│  └─────────────────────────────────────┘                                        │
│                                                                                  │
└─────────────────────────────────────────────────────────────────────────────────┘
```

### Entity Relationships (C# Models)

```csharp
// Base User Entity
public class User
{
    public Guid Id { get; set; }
    public Guid IdentityUserId { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? PhoneNumber { get; set; }
    public UserType UserType { get; set; }
    public UserStatus Status { get; set; }
    public string? AvatarUrl { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    // Navigation Properties (One-to-One, only one will be populated based on UserType)
    public Customer? Customer { get; set; }
    public Partner? Partner { get; set; }
    public Driver? Driver { get; set; }
    public AdminUser? AdminUser { get; set; }
    
    // One-to-Many
    public ICollection<Address> Addresses { get; set; } = new List<Address>();
}

// Customer Profile
public class Customer
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; }
    
    public Guid? PreferredPaymentMethodId { get; set; }
    public int LoyaltyPoints { get; set; }
    public CustomerPreferences? Preferences { get; set; } // JSON column
    public DateTime? LastOrderAt { get; set; }
}

// Partner Profile (Restaurant/Store Owner)
public class Partner
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; }
    
    public string BusinessName { get; set; }
    public BusinessType BusinessType { get; set; }
    public string? TaxId { get; set; }
    public decimal CommissionRate { get; set; }
    public VerificationStatus VerificationStatus { get; set; }
    public PartnerDocuments? Documents { get; set; } // JSON column
    public BankAccountInfo? BankAccount { get; set; } // JSON column
    public DateTime? VerifiedAt { get; set; }
}

// Driver Profile
public class Driver
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; }
    
    public string LicenseNumber { get; set; }
    public DateTime LicenseExpiry { get; set; }
    public VehicleType VehicleType { get; set; }
    public string VehiclePlate { get; set; }
    public string? VehicleModel { get; set; }
    public int? VehicleYear { get; set; }
    public string? InsuranceNumber { get; set; }
    public DateTime? InsuranceExpiry { get; set; }
    public BackgroundCheckStatus BackgroundCheckStatus { get; set; }
    public DriverStatus Status { get; set; }
    public double? CurrentLatitude { get; set; }
    public double? CurrentLongitude { get; set; }
    public bool IsOnline { get; set; }
    public decimal Rating { get; set; }
    public int TotalDeliveries { get; set; }
}

// Admin User Profile
public class AdminUser
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; }
    
    public string? Department { get; set; }
    public AdminRole Role { get; set; }
    public List<string> Permissions { get; set; } = new();
    public DateTime? LastLoginAt { get; set; }
    public bool IsSuperAdmin { get; set; }
}
```

### EF Core Configuration

```csharp
// UserConfiguration.cs
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);
        builder.HasIndex(u => u.IdentityUserId).IsUnique();
        builder.HasIndex(u => u.Email).IsUnique();
        
        builder.Property(u => u.UserType)
            .HasConversion<string>();
        
        // One-to-One relationships
        builder.HasOne(u => u.Customer)
            .WithOne(c => c.User)
            .HasForeignKey<Customer>(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade);
            
        builder.HasOne(u => u.Partner)
            .WithOne(p => p.User)
            .HasForeignKey<Partner>(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);
            
        builder.HasOne(u => u.Driver)
            .WithOne(d => d.User)
            .HasForeignKey<Driver>(d => d.UserId)
            .OnDelete(DeleteBehavior.Cascade);
            
        builder.HasOne(u => u.AdminUser)
            .WithOne(a => a.User)
            .HasForeignKey<AdminUser>(a => a.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        // One-to-Many
        builder.HasMany(u => u.Addresses)
            .WithOne(a => a.User)
            .HasForeignKey(a => a.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
```

## MVP Implementation Plan

### Phase 1: Core Services (Week 1-2)

#### Minimal Service Architecture
```
┌─────────────────────────────────────────────────────────────────┐
│                        API Gateway                               │
│                    (YARP / .NET Minimal API)                     │
└──────────────────────────┬──────────────────────────────────────┘
                           │
         ┌─────────────────┼─────────────────┐
         ▼                 ▼                 ▼
┌─────────────────┐ ┌─────────────────┐ ┌─────────────────┐
│ Identity Service│ │  Order Service  │ │ Delivery Service│
│   (Auth/Users)  │ │  (Orders/Menu)  │ │ (Tracking/Assign)│
└─────────────────┘ └─────────────────┘ └─────────────────┘
         │                 │                 │
         └─────────────────┴─────────────────┘
                           │
                    ┌──────▼──────┐
                    │  PostgreSQL  │
                    │  (Single DB) │
                    └─────────────┘
```

### Phase 2: MVP Features

#### Identity Service (Minimal)
- Email/password registration & login
- JWT token generation
- Basic user roles (Customer, Driver, Admin)

#### Order Service (Minimal)
- Create order with items
- View order status
- List orders by user

#### Delivery Service (Minimal)
- Assign driver to order
- Update delivery status
- Basic location tracking

### MVP Database Schema

```sql
-- Single database with schemas for MVP
CREATE SCHEMA identity;
CREATE SCHEMA orders;
CREATE SCHEMA delivery;

-- identity.users
CREATE TABLE identity.users (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    email VARCHAR(255) UNIQUE NOT NULL,
    password_hash VARCHAR(255) NOT NULL,
    role VARCHAR(50) NOT NULL, -- Customer, Driver, Admin
    name VARCHAR(255),
    phone VARCHAR(50),
    created_at TIMESTAMP DEFAULT NOW()
);

-- orders.orders
CREATE TABLE orders.orders (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    customer_id UUID REFERENCES identity.users(id),
    status VARCHAR(50) DEFAULT 'pending', -- pending, confirmed, preparing, ready, picked_up, delivered, cancelled
    total_amount DECIMAL(10,2),
    delivery_address TEXT,
    delivery_lat DECIMAL(10,8),
    delivery_lng DECIMAL(11,8),
    created_at TIMESTAMP DEFAULT NOW()
);

-- orders.order_items
CREATE TABLE orders.order_items (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    order_id UUID REFERENCES orders.orders(id),
    name VARCHAR(255),
    quantity INT,
    price DECIMAL(10,2)
);

-- delivery.assignments
CREATE TABLE delivery.assignments (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    order_id UUID REFERENCES orders.orders(id),
    driver_id UUID REFERENCES identity.users(id),
    status VARCHAR(50) DEFAULT 'assigned', -- assigned, picked_up, in_transit, delivered
    assigned_at TIMESTAMP DEFAULT NOW(),
    picked_up_at TIMESTAMP,
    delivered_at TIMESTAMP
);

-- delivery.driver_locations
CREATE TABLE delivery.driver_locations (
    driver_id UUID PRIMARY KEY REFERENCES identity.users(id),
    latitude DECIMAL(10,8),
    longitude DECIMAL(11,8),
    updated_at TIMESTAMP DEFAULT NOW()
);
```

### MVP API Endpoints

| Service | Method | Endpoint | Description |
|---------|--------|----------|-------------|
| Identity | POST | `/api/auth/register` | Register user |
| Identity | POST | `/api/auth/login` | Login, get JWT |
| Identity | GET | `/api/users/me` | Get current user |
| Orders | POST | `/api/orders` | Create order |
| Orders | GET | `/api/orders/{id}` | Get order details |
| Orders | GET | `/api/orders` | List user's orders |
| Orders | PATCH | `/api/orders/{id}/status` | Update order status |
| Delivery | POST | `/api/delivery/assign` | Assign driver |
| Delivery | PATCH | `/api/delivery/{id}/status` | Update delivery status |
| Delivery | PUT | `/api/drivers/location` | Update driver location |
| Delivery | GET | `/api/orders/{id}/tracking` | Get delivery tracking |

### MVP Project Structure

```
/src
├── DeliverMVP.Api/                 # Single API project
│   ├── Program.cs
│   ├── Controllers/
│   │   ├── AuthController.cs
│   │   ├── OrdersController.cs
│   │   └── DeliveryController.cs
│   ├── Services/
│   │   ├── AuthService.cs
│   │   ├── OrderService.cs
│   │   └── DeliveryService.cs
│   ├── Models/
│   │   ├── User.cs
│   │   ├── Order.cs
│   │   └── DeliveryAssignment.cs
│   └── Data/
│       └── AppDbContext.cs
├── docker-compose.yml
└── README.md
```

### Quick Start Commands

```bash
# Start infrastructure
docker-compose up -d postgres

# Run API
cd src/DeliverMVP.Api
dotnet run

# API available at http://localhost:5000
```

### docker-compose.yml

```yaml
version: '3.8'
services:
  postgres:
    image: postgres:16-alpine
    environment:
      POSTGRES_DB: delivermvp
      POSTGRES_USER: admin
      POSTGRES_PASSWORD: admin123
    ports:
      - "5432:5432"
    volumes:
      - postgres_data:/var/lib/postgresql/data

  api:
    build: ./src/DeliverMVP.Api
    ports:
      - "5000:8080"
    environment:
      - ConnectionStrings__Default=Host=postgres;Database=delivermvp;Username=admin;Password=admin123
    depends_on:
      - postgres

volumes:
  postgres_data:
```

### MVP Timeline

| Week | Deliverable |
|------|-------------|
| 1 | Auth + User registration/login |
| 2 | Order creation + listing |
| 3 | Driver assignment + status updates |
| 4 | Basic tracking + testing |

### Post-MVP Expansion Path
1. Split into separate microservices
2. Add message queue (RabbitMQ)
3. Add real-time updates (SignalR)
4. Add payment integration
5. Add partner (restaurant) portal
6. Add mobile apps

## MVP Implementation Without Partner Integration

Since the MVP doesn't include restaurant or grocery store partners with product catalogs, here's how to implement the order flow:

### Simplified Order Model

Instead of selecting products from a partner's menu, customers will:
1. **Manual Order Entry**: Customer describes what they want (text-based)
2. **Fixed Delivery Service**: Focus on point-to-point delivery (pickup from location A, deliver to location B)
3. **Price Estimation**: Admin/system sets delivery fee based on distance

### Modified Database Schema

```sql
-- Simplified orders table for MVP
CREATE TABLE orders.orders (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    customer_id UUID REFERENCES identity.users(id),
    
    -- Pickup details (where driver picks up)
    pickup_address TEXT NOT NULL,
    pickup_latitude DECIMAL(10,8),
    pickup_longitude DECIMAL(11,8),
    pickup_instructions TEXT,
    pickup_contact_name VARCHAR(255),
    pickup_contact_phone VARCHAR(20),
    
    -- Delivery details
    delivery_address TEXT NOT NULL,
    delivery_latitude DECIMAL(10,8),
    delivery_longitude DECIMAL(11,8),
    delivery_instructions TEXT,
    
    -- Order details (free-form for MVP)
    description TEXT NOT NULL,  -- "Pick up package from John's house"
    estimated_value DECIMAL(10,2),  -- Optional: value of items
    
    -- Pricing
    delivery_fee DECIMAL(10,2) NOT NULL,
    total DECIMAL(10,2) NOT NULL,
    
    status VARCHAR(50) DEFAULT 'pending',
    created_at TIMESTAMP DEFAULT NOW()
);
```

### MVP Use Cases

| Use Case | Description |
|----------|-------------|
| Package Delivery | "Pick up a box from 123 Main St, deliver to 456 Oak Ave" |
| Document Courier | "Collect documents from office, deliver to client" |
| Personal Errands | "Buy items from this store (customer pays), deliver to me" |

### Future Partner Integration Path

When ready to add partners:
1. Create `partners` and `products` tables
2. Add `partner_id` foreign key to orders
3. Build partner portal for menu/inventory management
4. Update order flow to select from partner catalogs