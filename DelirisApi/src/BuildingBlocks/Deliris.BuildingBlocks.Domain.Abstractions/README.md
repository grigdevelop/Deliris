# Deliris.BuildingBlocks.Domain.Abstractions

Core abstractions for the domain layer following Clean Architecture and Domain-Driven Design principles.

## Overview

This project contains all interfaces, abstract base classes, and contracts that define the domain layer's structure. It has **no dependencies** on infrastructure, frameworks, or implementation details, making it the foundation for domain-driven design across the Deliris solution.

## Architecture Principles

### Dependency Inversion
- All abstractions are defined here
- Implementation projects depend on this project
- This project depends on nothing except .NET base libraries

### Framework Agnostic
- No EF Core dependencies
- No ASP.NET Core dependencies
- No third-party framework dependencies
- Pure domain logic and contracts

## Structure

### Entities (`Entities/`)
Core entity abstractions for domain modeling:

- **IEntity<TId>** - Base interface for entities with identity
- **Entity<TId>** - Abstract base class with identity-based equality
- **IAggregateRoot** - Marker interface for aggregate roots
- **AggregateRoot<TId>** - Base class with domain event support
- **IAuditableEntity** - Interface for audit tracking
- **AuditableEntity<TId>** - Base class with audit fields
- **AuditableAggregateRoot<TId>** - Combines audit and domain events
- **ISoftDeletable** - Interface for soft deletion support

### Value Objects (`ValueObjects/`)
- **ValueObject** - Abstract base class for immutable value objects with structural equality

### Domain Events (`Events/`)
Event-driven architecture support:

- **IDomainEvent** - Base interface for domain events
- **DomainEvent** - Abstract record base class for events
- **IDomainEventHandler<TEvent>** - Interface for event handlers
- **IDomainEventDispatcher** - Interface for event dispatching

### Repositories (`Repositories/`)
Repository pattern abstractions:

- **IReadRepository<TEntity, TId>** - Read-only repository operations
- **IRepository<TEntity, TId>** - Full CRUD repository operations
- **IUnitOfWork** - Transaction coordination interface

### Specifications (`Specifications/`)
Specification pattern for complex queries:

- **ISpecification<T>** - Specification interface
- **Specification<T>** - Base class with fluent API for building queries

### Business Rules (`BusinessRules/`)
- **IBusinessRule** - Interface for domain business rules

### Enumerations (`Enumerations/`)
- **Enumeration** - Type-safe enumeration base class

### Common Types (`Common/`)
Shared domain types:

- **Result / Result<T>** - Result pattern for functional error handling
- **Error** - Domain error representation
- **PagedList<T>** - Pagination support with metadata

### Exceptions (`Exceptions/`)
- **DomainException** - Base class for domain exceptions

## Usage

### Entity Example
```csharp
using Deliris.BuildingBlocks.Domain.Abstractions.Entities;

public class Product : AggregateRoot<Guid>
{
    public string Name { get; private set; }
    public decimal Price { get; private set; }
    
    public Product(Guid id, string name, decimal price) : base(id)
    {
        Name = name;
        Price = price;
        RaiseDomainEvent(new ProductCreatedEvent(id, name));
    }
}
```

### Value Object Example
```csharp
using Deliris.BuildingBlocks.Domain.Abstractions.ValueObjects;

public class Money : ValueObject
{
    public decimal Amount { get; }
    public string Currency { get; }
    
    public Money(decimal amount, string currency)
    {
        Amount = amount;
        Currency = currency;
    }
    
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Amount;
        yield return Currency;
    }
}
```

### Repository Interface Example
```csharp
using Deliris.BuildingBlocks.Domain.Abstractions.Repositories;

public interface IProductRepository : IRepository<Product, Guid>
{
    Task<Product?> GetBySkuAsync(string sku, CancellationToken cancellationToken = default);
}
```

### Domain Event Example
```csharp
using Deliris.BuildingBlocks.Domain.Abstractions.Events;

public record ProductCreatedEvent : DomainEvent
{
    public Guid ProductId { get; }
    public string Name { get; }
    
    public ProductCreatedEvent(Guid productId, string name)
    {
        ProductId = productId;
        Name = name;
    }
}
```

## Design Decisions

### Why Abstractions are Separate
1. **Dependency Inversion** - Implementations depend on abstractions, not vice versa
2. **Reusability** - Multiple implementations can use the same abstractions
3. **Testability** - Easy to mock and test without implementation details
4. **Clean Architecture** - Clear separation of concerns and layers

### Why Some Classes are Abstract (Not Interfaces)
Classes like `Entity<TId>`, `ValueObject`, and `AggregateRoot<TId>` are abstract classes because they:
- Provide shared implementation logic (equality, hash codes, event management)
- Define protected methods for derived classes
- Enforce consistent behavior across all implementations

### Framework Independence
This project intentionally has no framework dependencies to ensure:
- Domain logic remains pure and focused
- Easy migration between frameworks
- No coupling to infrastructure concerns
- Maximum flexibility for different implementations

## Dependencies

- **.NET 10.0** only
- No external packages
- No framework dependencies

## Related Projects

- **Deliris.BuildingBlocks.Domain** - Concrete implementations and utilities
- **Deliris.BuildingBlocks.Infrastructure** - Infrastructure implementations (repositories, etc.)

## Best Practices

1. **Keep abstractions stable** - Changes here affect all implementations
2. **No implementation details** - Only contracts and base behavior
3. **Framework agnostic** - No infrastructure concerns
4. **Well documented** - All public APIs have XML documentation
5. **SOLID principles** - Especially Interface Segregation and Dependency Inversion

## License

This library is part of the Deliris project.
