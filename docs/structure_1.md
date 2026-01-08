BuildingBlocks/
├── Deliris.BuildingBlocks.Domain/
│   # Core domain primitives: Entity, AggregateRoot, ValueObject, DomainEvent interfaces
│
├── Deliris.BuildingBlocks.Application/
│   # CQRS abstractions: ICommand, IQuery, ICommandHandler, IQueryHandler, pipeline behaviors
│
├── Deliris.BuildingBlocks.Infrastructure/
│   # Cross-cutting concerns: UnitOfWork, Repository base, database context abstractions
│
├── Deliris.BuildingBlocks.EventBus/
│   # Integration events: IIntegrationEvent, IEventBus, event publishing/subscribing contracts
│
├── Deliris.BuildingBlocks.Messaging/
│   # Message broker abstractions: RabbitMQ/Azure Service Bus wrappers, outbox pattern
│
└── Deliris.BuildingBlocks.Common/
    # Shared utilities: Result pattern, pagination, guards, extension methods


Deliris.BuildingBlocks.Domain/
├── Primitives/
│   ├── Entity.cs                    # Base class with Id, equality by identifier
│   ├── AggregateRoot.cs             # Entity with domain events collection, event raising
│   ├── ValueObject.cs               # Immutable object with structural equality
│   └── Enumeration.cs               # Type-safe enum pattern base class
│
├── Events/
│   ├── IDomainEvent.cs              # Marker interface for domain events with timestamp
│   └── IDomainEventHandler.cs       # Handler contract for processing domain events
│
├── Exceptions/
│   ├── DomainException.cs           # Base exception for domain rule violations
│   └── BusinessRuleValidationException.cs  # Exception when business rule fails
│
├── Rules/
│   ├── IBusinessRule.cs             # Contract for business rule validation
│   └── BusinessRuleChecker.cs       # Helper to check and throw on rule violations
│
├── Specifications/
│   ├── Specification.cs             # Base specification pattern for domain queries
│   └── ISpecification.cs            # Interface for composable query specifications
│
└── Auditing/
    ├── IAuditableEntity.cs          # Interface for CreatedAt, ModifiedAt tracking
    └── ISoftDeletable.cs            # Interface for soft delete with IsDeleted, DeletedAt


/// <summary>
/// Represents a recorded domain event with its associated data and ordering information.
/// Used for tracking and dispatching domain events in the correct sequence.
/// </summary>
public class DomainEventRecord
{
    /// <summary>
    /// Gets the domain event data payload.
    /// </summary>
    public object EventData { get; }

    /// <summary>
    /// Gets the sequential order of this event for maintaining event ordering.
    /// </summary>
    public long EventOrder { get; }

    public DomainEventRecord(object eventData, long eventOrder)
    {
        EventData = eventData;
        EventOrder = eventOrder;
    }
}

// BuildingBlocks/EventBus/EventBus.Abstractions/IEventBus.cs
namespace BuildingBlocks.EventBus.Abstractions
{
    public interface IEventBus
    {
        Task PublishAsync<T>(T @event) where T : IntegrationEvent;
        void Subscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>;
    }
}

// BuildingBlocks/EventBus/EventBus.Abstractions/IntegrationEvent.cs
namespace BuildingBlocks.EventBus.Abstractions
{
    public record IntegrationEvent
    {
        public Guid Id { get; init; }
        public DateTime CreatedAt { get; init; }

        public IntegrationEvent()
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
        }
    }
}

// BuildingBlocks/EventBus/EventBus.Abstractions/IIntegrationEventHandler.cs
namespace BuildingBlocks.EventBus.Abstractions
{
    public interface IIntegrationEventHandler<in TIntegrationEvent> 
        where TIntegrationEvent : IntegrationEvent
    {
        Task Handle(TIntegrationEvent @event);
    }
}

// BuildingBlocks/Logging/Logging.csproj
<Project Sdk="Microsoft.NET.Sdk">
    <PropertyGroup>
        <TargetFramework>net8.0</TargetFramework>
    </PropertyGroup>
    <ItemGroup>
        <PackageReference Include="Serilog.AspNetCore" Version="8.0.0" />
        <PackageReference Include="Serilog.Sinks.Console" Version="5.0.0" />
        <PackageReference Include="Serilog.Sinks.Elasticsearch" Version="9.0.0" />
    </ItemGroup>
</Project>

// BuildingBlocks/WebHost/WebHostExtensions.cs
namespace BuildingBlocks.WebHost
{
    public static class WebHostExtensions
    {
        public static IHostBuilder UseCustomSerilog(this IHostBuilder builder)
        {
            return builder.UseSerilog((context, services, configuration) =>
            {
                configuration
                    .ReadFrom.Configuration(context.Configuration)
                    .Enrich.FromLogContext()
                    .WriteTo.Console();
            });
        }
    }
}


Create a comprehensive Domain layer implementation for Deliris.BuildingBlocks.Domain following DDD (Domain-Driven Design) best practices and clean architecture principles. Include:

1. Base entity classes with proper identity handling and equality comparison
2. Value objects with immutability and validation
3. Domain events infrastructure for capturing state changes
4. Aggregate root pattern implementation
5. Repository interfaces following repository pattern
6. Domain exceptions for business rule violations
7. Specification pattern for complex queries
8. Audit fields (CreatedAt, UpdatedAt, CreatedBy, UpdatedBy) support

Ensure all classes are:
- Immutable where appropriate (value objects, domain events)
- Thread-safe
- Well-documented with XML comments
- Following SOLID principles
- Using nullable reference types correctly
- Implementing proper encapsulation with private setters
- Including validation in constructors
- Supporting both EF Core and other ORMs through proper design

Generate the complete domain primitives including Entity<TId>, ValueObject, DomainEvent, AggregateRoot<TId>, IAuditableEntity, and related infrastructure.


Deliris.BuildingBlocks.Application/
├── Commands/
│   ├── ICommand.cs                  # Marker interface for commands (write operations)
│   ├── ICommandHandler.cs           # Handler contract for processing commands
│   └── CommandResult.cs             # Result wrapper with success/failure status
│
├── Queries/
│   ├── IQuery.cs                    # Marker interface for queries (read operations)
│   ├── IQueryHandler.cs             # Handler contract for processing queries
│   └── PagedQuery.cs                # Base class for paginated queries
│
├── Behaviors/
│   ├── ValidationBehavior.cs        # FluentValidation pipeline behavior
│   ├── LoggingBehavior.cs           # Request/response logging behavior
│   ├── PerformanceBehavior.cs       # Performance monitoring behavior
│   └── TransactionBehavior.cs       # Unit of work transaction behavior
│
├── Exceptions/
│   ├── ValidationException.cs       # Exception for validation failures
│   ├── NotFoundException.cs         # Exception for entity not found
│   └── ApplicationException.cs      # Base application layer exception
│
├── Models/
│   ├── Result.cs                    # Generic result pattern implementation
│   ├── PagedResult.cs               # Paginated result wrapper
│   └── Error.cs                     # Error details for failed operations
│
├── Contracts/
│   ├── IUnitOfWork.cs               # Unit of work pattern interface
│   ├── IRepository.cs               # Generic repository interface
│   └── IEventDispatcher.cs          # Domain event dispatcher interface
│
└── Extensions/
    ├── ServiceCollectionExtensions.cs  # DI registration helpers
    └── MediatorExtensions.cs          # MediatR configuration extensions

// Application/Commands/ICommand.cs
namespace Deliris.BuildingBlocks.Application.Commands
{
    /// <summary>
    /// Marker interface for commands that don't return a value.
    /// Commands represent write operations that change system state.
    /// </summary>
    public interface ICommand : IRequest<Result>
    {
    }

    /// <summary>
    /// Marker interface for commands that return a value of type TResponse.
    /// </summary>
    /// <typeparam name="TResponse">The type of response returned by the command.</typeparam>
    public interface ICommand<out TResponse> : IRequest<Result<TResponse>>
    {
    }
}

// Application/Commands/ICommandHandler.cs
namespace Deliris.BuildingBlocks.Application.Commands
{
    /// <summary>
    /// Handler interface for processing commands without return values.
    /// </summary>
    /// <typeparam name="TCommand">The type of command to handle.</typeparam>
    public interface ICommandHandler<in TCommand> : IRequestHandler<TCommand, Result>
        where TCommand : ICommand
    {
    }

    /// <summary>
    /// Handler interface for processing commands with return values.
    /// </summary>
    /// <typeparam name="TCommand">The type of command to handle.</typeparam>
    /// <typeparam name="TResponse">The type of response returned.</typeparam>
    public interface ICommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, Result<TResponse>>
        where TCommand : ICommand<TResponse>
    {
    }
}

// Application/Queries/IQuery.cs
namespace Deliris.BuildingBlocks.Application.Queries
{
    /// <summary>
    /// Marker interface for queries that return data without modifying state.
    /// </summary>
    /// <typeparam name="TResponse">The type of data returned by the query.</typeparam>
    public interface IQuery<out TResponse> : IRequest<Result<TResponse>>
    {
    }
}

// Application/Queries/IQueryHandler.cs
namespace Deliris.BuildingBlocks.Application.Queries
{
    /// <summary>
    /// Handler interface for processing queries.
    /// </summary>
    /// <typeparam name="TQuery">The type of query to handle.</typeparam>
    /// <typeparam name="TResponse">The type of response returned.</typeparam>
    public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
        where TQuery : IQuery<TResponse>
    {
    }
}

// Application/Queries/PagedQuery.cs
namespace Deliris.BuildingBlocks.Application.Queries
{
    /// <summary>
    /// Base class for paginated queries with sorting and filtering support.
    /// </summary>
    /// <typeparam name="TResponse">The type of items in the paged result.</typeparam>
    public abstract record PagedQuery<TResponse> : IQuery<PagedResult<TResponse>>
    {
        /// <summary>
        /// Gets the page number (1-based).
        /// </summary>
        public int PageNumber { get; init; } = 1;

        /// <summary>
        /// Gets the page size (number of items per page).
        /// </summary>
        public int PageSize { get; init; } = 10;

        /// <summary>
        /// Gets the field to sort by.
        /// </summary>
        public string? SortBy { get; init; }

        /// <summary>
        /// Gets the sort direction (asc or desc).
        /// </summary>
        public string SortDirection { get; init; } = "asc";

        /// <summary>
        /// Gets the search term for filtering.
        /// </summary>
        public string? SearchTerm { get; init; }
    }
}

// Application/Models/Result.cs
namespace Deliris.BuildingBlocks.Application.Models
{
    /// <summary>
    /// Represents the result of an operation without a return value.
    /// </summary>
    public class Result
    {
        /// <summary>
        /// Gets a value indicating whether the operation was successful.
        /// </summary>
        public bool IsSuccess { get; }

        /// <summary>
        /// Gets a value indicating whether the operation failed.
        /// </summary>
        public bool IsFailure => !IsSuccess;

        /// <summary>
        /// Gets the error details if the operation failed.
        /// </summary>
        public Error? Error { get; }

        protected Result(bool isSuccess, Error? error)
        {
            if (isSuccess && error != null)
                throw new InvalidOperationException("Successful result cannot have an error.");

            if (!isSuccess && error == null)
                throw new InvalidOperationException("Failed result must have an error.");

            IsSuccess = isSuccess;
            Error = error;
        }

        /// <summary>
        /// Creates a successful result.
        /// </summary>
        public static Result Success() => new(true, null);

        /// <summary>
        /// Creates a failed result with the specified error.
        /// </summary>
        public static Result Failure(Error error) => new(false, error);

        /// <summary>
        /// Creates a successful result with a value.
        /// </summary>
        public static Result<TValue> Success<TValue>(TValue value) => new(value, true, null);

        /// <summary>
        /// Creates a failed result with the specified error.
        /// </summary>
        public static Result<TValue> Failure<TValue>(Error error) => new(default, false, error);
    }

    /// <summary>
    /// Represents the result of an operation with a return value.
    /// </summary>
    /// <typeparam name="TValue">The type of the return value.</typeparam>
    public class Result<TValue> : Result
    {
        private readonly TValue? _value;

        /// <summary>
        /// Gets the value if the operation was successful.
        /// </summary>
        public TValue Value => IsSuccess
            ? _value!
            : throw new InvalidOperationException("Cannot access value of a failed result.");

        internal Result(TValue? value, bool isSuccess, Error? error)
            : base(isSuccess, error)
        {
            _value = value;
        }

        /// <summary>
        /// Implicitly converts a value to a successful result.
        /// </summary>
        public static implicit operator Result<TValue>(TValue value) => Success(value);
    }
}

// Application/Models/Error.cs
namespace Deliris.BuildingBlocks.Application.Models
{
    /// <summary>
    /// Represents an error with a code and message.
    /// </summary>
    public sealed record Error
    {
        /// <summary>
        /// Gets the error code.
        /// </summary>
        public string Code { get; }

        /// <summary>
        /// Gets the error message.
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Gets the error type.
        /// </summary>
        public ErrorType Type { get; }

        public Error(string code, string message, ErrorType type = ErrorType.Failure)
        {
            Code = code;
            Message = message;
            Type = type;
        }

        public static Error None => new(string.Empty, string.Empty, ErrorType.None);
        public static Error NullValue => new("Error.NullValue", "Null value was provided", ErrorType.Validation);
    }

    /// <summary>
    /// Represents the type of error.
    /// </summary>
    public enum ErrorType
    {
        None = 0,
        Failure = 1,
        Validation = 2,
        NotFound = 3,
        Conflict = 4,
        Unauthorized = 5,
        Forbidden = 6
    }
}

// Application/Models/PagedResult.cs
namespace Deliris.BuildingBlocks.Application.Models
{
    /// <summary>
    /// Represents a paginated result set.
    /// </summary>
    /// <typeparam name="T">The type of items in the result.</typeparam>
    public sealed class PagedResult<T>
    {
        /// <summary>
        /// Gets the items in the current page.
        /// </summary>
        public IReadOnlyList<T> Items { get; }

        /// <summary>
        /// Gets the current page number (1-based).
        /// </summary>
        public int PageNumber { get; }

        /// <summary>
        /// Gets the page size.
        /// </summary>
        public int PageSize { get; }

        /// <summary>
        /// Gets the total number of items across all pages.
        /// </summary>
        public int TotalCount { get; }

        /// <summary>
        /// Gets the total number of pages.
        /// </summary>
        public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);

        /// <summary>
        /// Gets a value indicating whether there is a previous page.
        /// </summary>
        public bool HasPreviousPage => PageNumber > 1;

        /// <summary>
        /// Gets a value indicating whether there is a next page.
        /// </summary>
        public bool HasNextPage => PageNumber < TotalPages;

        public PagedResult(IReadOnlyList<T> items, int pageNumber, int pageSize, int totalCount)
        {
            Items = items;
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalCount = totalCount;
        }

        /// <summary>
        /// Creates an empty paged result.
        /// </summary>
        public static PagedResult<T> Empty() => new(Array.Empty<T>(), 1, 10, 0);
    }
}

// Application/Behaviors/ValidationBehavior.cs
namespace Deliris.BuildingBlocks.Application.Behaviors
{
    /// <summary>
    /// Pipeline behavior that validates requests using FluentValidation.
    /// </summary>
    public sealed class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            if (!_validators.Any())
            {
                return await next();
            }

            var context = new ValidationContext<TRequest>(request);

            var validationResults = await Task.WhenAll(
                _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

            var failures = validationResults
                .Where(r => !r.IsValid)
                .SelectMany(r => r.Errors)
                .ToList();

            if (failures.Any())
            {
                throw new ValidationException(failures);
            }

            return await next();
        }
    }
}

// Application/Behaviors/LoggingBehavior.cs
namespace Deliris.BuildingBlocks.Application.Behaviors
{
    /// <summary>
    /// Pipeline behavior that logs request and response information.
    /// </summary>
    public sealed class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

        public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            var requestName = typeof(TRequest).Name;

            _logger.LogInformation(
                "Handling {RequestName} at {DateTimeUtc}",
                requestName,
                DateTime.UtcNow);

            try
            {
                var response = await next();

                _logger.LogInformation(
                    "Handled {RequestName} successfully at {DateTimeUtc}",
                    requestName,
                    DateTime.UtcNow);

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error handling {RequestName} at {DateTimeUtc}",
                    requestName,
                    DateTime.UtcNow);

                throw;
            }
        }
    }
}

// Application/Behaviors/PerformanceBehavior.cs
namespace Deliris.BuildingBlocks.Application.Behaviors
{
    /// <summary>
    /// Pipeline behavior that monitors request performance and logs slow requests.
    /// </summary>
    public sealed class PerformanceBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<PerformanceBehavior<TRequest, TResponse>> _logger;
        private readonly Stopwatch _timer;
        private const int SlowRequestThresholdMs = 500;

        public PerformanceBehavior(ILogger<PerformanceBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger;
            _timer = new Stopwatch();
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            _timer.Start();

            var response = await next();

            _timer.Stop();

            var elapsedMilliseconds = _timer.ElapsedMilliseconds;

            if (elapsedMilliseconds > SlowRequestThresholdMs)
            {
                var requestName = typeof(TRequest).Name;

                _logger.LogWarning(
                    "Slow request detected: {RequestName} took {ElapsedMilliseconds}ms",
                    requestName,
                    elapsedMilliseconds);
            }

            return response;
        }
    }
}

// Application/Behaviors/TransactionBehavior.cs
namespace Deliris.BuildingBlocks.Application.Behaviors
{
    /// <summary>
    /// Pipeline behavior that wraps command execution in a database transaction.
    /// </summary>
    public


Deliris.IdentityService.Domain implementation details with using Deliris.BuildingBlocks.Domain.Abstractions (or also maybe Deliris.BuildingBlocks.Domain) and Microsoft.AspNetCore.Identity.EntityFrameworkCore.


## Deliris.IdentityService.Domain Implementation Details

This section outlines the implementation approach for the Identity Service Domain layer, utilizing:
- `Deliris.BuildingBlocks.Domain.Abstractions` (and potentially `Deliris.BuildingBlocks.Domain`)
- `Microsoft.AspNetCore.Identity.EntityFrameworkCore`

The implementation should balance DDD principles from the BuildingBlocks with ASP.NET Core Identity's requirements for authentication and authorization.

### Architecture Overview

The Identity Service Domain layer will be structured to maintain DDD principles while integrating with ASP.NET Core Identity. The approach involves:

1. **Entity Design Strategy**:
   - Custom identity entities that extend ASP.NET Core Identity base classes
   - Integration with `AuditableAggregateRoot<TId>` from BuildingBlocks for domain events and auditing
   - Use of Value Objects for complex properties (e.g., Email, PhoneNumber)

2. **Hybrid Approach**:
   - Identity entities will NOT directly inherit from BuildingBlocks domain primitives
   - Instead, use composition and domain events to maintain DDD patterns
   - Leverage ASP.NET Core Identity's built-in features for authentication/authorization
   - Apply BuildingBlocks patterns for business logic and domain events

### Project Structure

```
Deliris.IdentityService.Domain/
├── Entities/
│   ├── User.cs                      # Custom IdentityUser with domain logic
│   ├── Role.cs                      # Custom IdentityRole with domain logic
│   ├── UserRole.cs                  # Many-to-many relationship
│   ├── UserClaim.cs                 # User claims
│   ├── RoleClaim.cs                 # Role claims
│   ├── UserLogin.cs                 # External login providers
│   └── UserToken.cs                 # Authentication tokens
│
├── ValueObjects/
│   ├── Email.cs                     # Email value object with validation
│   ├── PhoneNumber.cs               # Phone number value object
│   └── UserProfile.cs               # User profile information
│
├── Events/
│   ├── UserRegisteredEvent.cs       # Domain event for user registration
│   ├── UserEmailChangedEvent.cs     # Domain event for email change
│   ├── UserProfileUpdatedEvent.cs   # Domain event for profile updates
│   └── UserDeactivatedEvent.cs      # Domain event for user deactivation
│
├── Repositories/
│   ├── IUserRepository.cs           # User repository interface
│   ├── IRoleRepository.cs           # Role repository interface
│   └── IIdentityUnitOfWork.cs       # Unit of work for identity operations
│
├── Exceptions/
│   ├── UserAlreadyExistsException.cs
│   ├── InvalidEmailException.cs
│   └── UserNotFoundException.cs
│
└── Constants/
    └── IdentityConstants.cs         # Identity-related constants
```

### Implementation Details

#### 1. Custom User Entity

The `User` entity extends `IdentityUser<Guid>` and incorporates domain events:

```csharp
// Entities/User.cs
using Microsoft.AspNetCore.Identity;
using Deliris.BuildingBlocks.Domain.Abstractions;

namespace Deliris.IdentityService.Domain.Entities;

public class User : IdentityUser<Guid>
{
    private readonly List<IDomainEvent> _domainEvents = new();

    // Value Objects
    public Email EmailAddress { get; private set; } = null!;
    public UserProfile Profile { get; private set; } = null!;

    // Audit Fields
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public string? CreatedBy { get; private set; }
    public string? UpdatedBy { get; private set; }

    // Soft Delete
    public bool IsDeleted { get; private set; }
    public DateTime? DeletedAt { get; private set; }

    // Domain Events
    public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    // Navigation Properties
    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    public virtual ICollection<UserClaim> Claims { get; set; } = new List<UserClaim>();
    public virtual ICollection<UserLogin> Logins { get; set; } = new List<UserLogin>();
    public virtual ICollection<UserToken> Tokens { get; set; } = new List<UserToken>();

    // Private constructor for EF Core
    private User() { }

    // Factory method for user registration
    public static User Register(
        string username,
        Email email,
        UserProfile profile,
        string? createdBy = null)
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            UserName = username,
            EmailAddress = email,
            Email = email.Value,
            Profile = profile,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = createdBy,
            SecurityStamp = Guid.NewGuid().ToString(),
            ConcurrencyStamp = Guid.NewGuid().ToString()
        };

        user.RaiseDomainEvent(new UserRegisteredEvent(
            user.Id,
            user.UserName,
            user.EmailAddress.Value,
            user.CreatedAt));

        return user;
    }

    public void UpdateProfile(UserProfile newProfile, string? updatedBy = null)
    {
        Profile = newProfile;
        UpdatedAt = DateTime.UtcNow;
        UpdatedBy = updatedBy;

        RaiseDomainEvent(new UserProfileUpdatedEvent(Id, Profile, UpdatedAt.Value));
    }

    public void ChangeEmail(Email newEmail, string? updatedBy = null)
    {
        var oldEmail = EmailAddress.Value;
        EmailAddress = newEmail;
        Email = newEmail.Value;
        EmailConfirmed = false;
        UpdatedAt = DateTime.UtcNow;
        UpdatedBy = updatedBy;

        RaiseDomainEvent(new UserEmailChangedEvent(Id, oldEmail, newEmail.Value, UpdatedAt.Value));
    }

    public void Deactivate(string? deletedBy = null)
    {
        IsDeleted = true;
        DeletedAt = DateTime.UtcNow;
        UpdatedBy = deletedBy;

        RaiseDomainEvent(new UserDeactivatedEvent(Id, DeletedAt.Value));
    }

    public void ConfirmEmail()
    {
        EmailConfirmed = true;
        UpdatedAt = DateTime.UtcNow;
    }

    public void ConfirmPhoneNumber()
    {
        PhoneNumberConfirmed = true;
        UpdatedAt = DateTime.UtcNow;
    }

    private void RaiseDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}
```

#### 2. Custom Role Entity

```csharp
// Entities/Role.cs
using Microsoft.AspNetCore.Identity;

namespace Deliris.IdentityService.Domain.Entities;

public class Role : IdentityRole<Guid>
{
    public string? Description { get; set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public bool IsDeleted { get; private set; }

    // Navigation Properties
    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    public virtual ICollection<RoleClaim> RoleClaims { get; set; } = new List<RoleClaim>();

    private Role() { }

    public static Role Create(string name, string? description = null)
    {
        return new Role
        {
            Id = Guid.NewGuid(),
            Name = name,
            NormalizedName = name.ToUpperInvariant(),
            Description = description,
            CreatedAt = DateTime.UtcNow,
            ConcurrencyStamp = Guid.NewGuid().ToString()
        };
    }
}
```

#### 3. Supporting Entities

```csharp
// Entities/UserRole.cs
using Microsoft.AspNetCore.Identity;

namespace Deliris.IdentityService.Domain.Entities;

public class UserRole : IdentityUserRole<Guid>
{
    public virtual User User { get; set; } = null!;
    public virtual Role Role { get; set; } = null!;
}

// Entities/UserClaim.cs
public class UserClaim : IdentityUserClaim<Guid>
{
    public virtual User User { get; set; } = null!;
}

// Entities/RoleClaim.cs
public class RoleClaim : IdentityRoleClaim<Guid>
{
    public virtual Role Role { get; set; } = null!;
}

// Entities/UserLogin.cs
public class UserLogin : IdentityUserLogin<Guid>
{
    public virtual User User { get; set; } = null!;
}

// Entities/UserToken.cs
public class UserToken : IdentityUserToken<Guid>
{
    public virtual User User { get; set; } = null!;
}
```

#### 4. Value Objects

```csharp
// ValueObjects/Email.cs
using Deliris.BuildingBlocks.Domain.Abstractions;

namespace Deliris.IdentityService.Domain.ValueObjects;

public sealed class Email : ValueObject
{
    public string Value { get; }

    private Email(string value)
    {
        Value = value;
    }

    public static Result<Email> Create(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return Result.Failure<Email>(new Error(
                "Email.Empty",
                "Email cannot be empty",
                ErrorType.Validation));
        }

        email = email.Trim().ToLowerInvariant();

        if (!IsValidEmail(email))
        {
            return Result.Failure<Email>(new Error(
                "Email.Invalid",
                "Email format is invalid",
                ErrorType.Validation));
        }

        return Result.Success(new Email(email));
    }

    private static bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}

// ValueObjects/UserProfile.cs
public sealed class UserProfile : ValueObject
{
    public string FirstName { get; }
    public string LastName { get; }
    public string? DisplayName { get; }
    public DateTime? DateOfBirth { get; }

    private UserProfile(string firstName, string lastName, string? displayName, DateTime? dateOfBirth)
    {
        FirstName = firstName;
        LastName = lastName;
        DisplayName = displayName;
        DateOfBirth = dateOfBirth;
    }

    public static Result<UserProfile> Create(
        string firstName,
        string lastName,
        string? displayName = null,
        DateTime? dateOfBirth = null)
    {
        if (string.IsNullOrWhiteSpace(firstName))
        {
            return Result.Failure<UserProfile>(new Error(
                "UserProfile.FirstNameEmpty",
                "First name cannot be empty",
                ErrorType.Validation));
        }

        if (string.IsNullOrWhiteSpace(lastName))
        {
            return Result.Failure<UserProfile>(new Error(
                "UserProfile.LastNameEmpty",
                "Last name cannot be empty",
                ErrorType.Validation));
        }

        if (dateOfBirth.HasValue && dateOfBirth.Value > DateTime.UtcNow)
        {
            return Result.Failure<UserProfile>(new Error(
                "UserProfile.InvalidDateOfBirth",
                "Date of birth cannot be in the future",
                ErrorType.Validation));
        }

        return Result.Success(new UserProfile(
            firstName.Trim(),
            lastName.Trim(),
            displayName?.Trim(),
            dateOfBirth));
    }

    public string FullName => $"{FirstName} {LastName}";

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return FirstName;
        yield return LastName;
        yield return DisplayName ?? string.Empty;
        yield return DateOfBirth ?? DateTime.MinValue;
    }
}
```

#### 5. Domain Events

```csharp
// Events/UserRegisteredEvent.cs
using Deliris.BuildingBlocks.Domain.Abstractions;

namespace Deliris.IdentityService.Domain.Events;

public sealed record UserRegisteredEvent(
    Guid UserId,
    string UserName,
    string Email,
    DateTime RegisteredAt) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

// Events/UserEmailChangedEvent.cs
public sealed record UserEmailChangedEvent(
    Guid UserId,
    string OldEmail,
    string NewEmail,
    DateTime ChangedAt) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

// Events/UserProfileUpdatedEvent.cs
public sealed record UserProfileUpdatedEvent(
    Guid UserId,
    UserProfile NewProfile,
    DateTime UpdatedAt) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

// Events/UserDeactivatedEvent.cs
public sealed record UserDeactivatedEvent(
    Guid UserId,
    DateTime DeactivatedAt) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
```

#### 6. Repository Interfaces

```csharp
// Repositories/IUserRepository.cs
namespace Deliris.IdentityService.Domain.Repositories;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<User?> GetByUserNameAsync(string userName, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<bool> ExistsByUserNameAsync(string userName, CancellationToken cancellationToken = default);
    Task AddAsync(User user, CancellationToken cancellationToken = default);
    void Update(User user);
    void Remove(User user);
}

// Repositories/IRoleRepository.cs
public interface IRoleRepository
{
    Task<Role?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Role?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<IEnumerable<Role>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(Role role, CancellationToken cancellationToken = default);
    void Update(Role role);
    void Remove(Role role);
}

// Repositories/IIdentityUnitOfWork.cs
public interface IIdentityUnitOfWork : IDisposable
{
    IUserRepository Users { get; }
    IRoleRepository Roles { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}
```

#### 7. Domain Exceptions
