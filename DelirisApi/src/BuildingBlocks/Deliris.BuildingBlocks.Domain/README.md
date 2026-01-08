# Deliris.BuildingBlocks.Domain

A comprehensive Domain layer implementation following Domain-Driven Design (DDD) best practices and Clean Architecture principles.

## Overview

This library provides essential building blocks for implementing domain models in a DDD-based application. It includes base classes, interfaces, and patterns that enforce domain logic encapsulation, business rule validation, and proper separation of concerns.

## Features

### Core Primitives

#### Entity<TId>
Base class for entities with identity-based equality comparison.

```csharp
public class Product : Entity<Guid>
{
    public string Name { get; private set; }
    public decimal Price { get; private set; }
    
    public Product(Guid id, string name, decimal price) : base(id)
    {
        Name = name;
        Price = price;
    }
}
```

#### ValueObject
Base class for immutable value objects with structural equality.

```csharp
public class Address : ValueObject
{
    public string Street { get; }
    public string City { get; }
    public string ZipCode { get; }
    
    public Address(string street, string city, string zipCode)
    {
        Street = street;
        City = city;
        ZipCode = zipCode;
    }
    
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Street;
        yield return City;
        yield return ZipCode;
    }
}
```

#### AggregateRoot<TId>
Base class for aggregate roots with domain event support.

```csharp
public class Order : AggregateRoot<Guid>
{
    public OrderStatus Status { get; private set; }
    
    public void Complete()
    {
        Status = OrderStatus.Completed;
        RaiseDomainEvent(new OrderCompletedEvent(Id));
    }
}
```

#### AuditableEntity<TId> & AuditableAggregateRoot<TId>
Base classes with built-in audit tracking (CreatedAt, UpdatedAt, CreatedBy, UpdatedBy).

```csharp
public class Customer : AuditableAggregateRoot<Guid>
{
    public string Name { get; private set; }
    
    public Customer(Guid id, string name) : base(id)
    {
        Name = name;
        SetCreatedAudit("system");
    }
}
```

### Domain Events

#### IDomainEvent & DomainEvent
Infrastructure for capturing and handling domain events.

```csharp
public record OrderCompletedEvent : DomainEvent
{
    public Guid OrderId { get; }
    
    public OrderCompletedEvent(Guid orderId)
    {
        OrderId = orderId;
    }
}
```

### Business Rules

#### IBusinessRule & BusinessRuleValidator
Pattern for encapsulating and validating business rules.

```csharp
public class MinimumOrderAmountRule : IBusinessRule
{
    private readonly decimal _amount;
    private readonly decimal _minimumAmount;
    
    public string Name => "MinimumOrderAmount";
    public string Message => $"Order amount must be at least {_minimumAmount}";
    
    public MinimumOrderAmountRule(decimal amount, decimal minimumAmount)
    {
        _amount = amount;
        _minimumAmount = minimumAmount;
    }
    
    public bool IsSatisfied() => _amount >= _minimumAmount;
}

// Usage
BusinessRuleValidator.CheckRule(new MinimumOrderAmountRule(orderAmount, 100));
```

### Repository Pattern

#### IRepository<TEntity, TId> & IReadRepository<TEntity, TId>
Interfaces for repository pattern implementation.

```csharp
public interface IProductRepository : IRepository<Product, Guid>
{
    Task<Product?> GetBySkuAsync(string sku, CancellationToken cancellationToken = default);
}
```

#### IUnitOfWork
Interface for coordinating transactions across multiple repositories.

### Specification Pattern

#### ISpecification<T> & Specification<T>
Pattern for encapsulating query logic in a reusable and composable way.

```csharp
public class ActiveProductsSpecification : Specification<Product>
{
    public ActiveProductsSpecification()
    {
        AddCriteria(p => p.IsActive);
        AddOrderBy(p => p.Name);
    }
}

// Composable specifications
var spec = new ActiveProductsSpecification()
    .And(new InStockProductsSpecification());
```

### Domain Exceptions

Comprehensive exception hierarchy for domain-related errors:

- **DomainException** - Base exception for all domain errors
- **BusinessRuleValidationException** - Business rule violations
- **EntityNotFoundException** - Entity not found errors
- **InvalidEntityStateException** - Invalid entity state errors
- **DomainValidationException** - Validation errors

```csharp
throw new EntityNotFoundException(typeof(Product), productId);
```

### Additional Patterns

#### Enumeration
Type-safe enumeration pattern for domain-specific enumerations.

```csharp
public class OrderStatus : Enumeration
{
    public static readonly OrderStatus Pending = new(1, "Pending");
    public static readonly OrderStatus Completed = new(2, "Completed");
    public static readonly OrderStatus Cancelled = new(3, "Cancelled");
    
    private OrderStatus(int id, string name) : base(id, name) { }
}

// Usage
var status = OrderStatus.FromId(1);
var allStatuses = OrderStatus.GetAll<OrderStatus>();
```

#### Result Pattern
Functional error handling pattern.

```csharp
public Result<Product> CreateProduct(string name, decimal price)
{
    if (string.IsNullOrWhiteSpace(name))
        return Result.Failure<Product>("Product name is required");
    
    if (price <= 0)
        return Result.Failure<Product>("Price must be greater than zero");
    
    var product = new Product(Guid.NewGuid(), name, price);
    return Result.Success(product);
}

// Usage
var result = CreateProduct("Widget", 19.99m);
if (result.IsSuccess)
{
    var product = result.Value;
    // Use product
}
else
{
    var error = result.Error;
    // Handle error
}
```

#### ISoftDeletable
Interface for entities supporting soft deletion.

```csharp
public class Product : Entity<Guid>, ISoftDeletable
{
    public bool IsDeleted { get; private set; }
    public DateTime? DeletedAtUtc { get; private set; }
    public string? DeletedBy { get; private set; }
    
    public void Delete(string? deletedBy, DateTime? deletedAtUtc = null)
    {
        IsDeleted = true;
        DeletedBy = deletedBy;
        DeletedAtUtc = deletedAtUtc ?? DateTime.UtcNow;
    }
    
    public void Restore()
    {
        IsDeleted = false;
        DeletedBy = null;
        DeletedAtUtc = null;
    }
}
```

## Design Principles

### SOLID Principles
- **Single Responsibility**: Each class has a single, well-defined purpose
- **Open/Closed**: Classes are open for extension but closed for modification
- **Liskov Substitution**: Derived classes can substitute base classes
- **Interface Segregation**: Focused, minimal interfaces
- **Dependency Inversion**: Depend on abstractions, not concretions

### DDD Principles
- **Ubiquitous Language**: Code reflects domain language
- **Bounded Contexts**: Clear boundaries and responsibilities
- **Aggregate Roots**: Consistency boundaries and entry points
- **Domain Events**: Capture significant domain occurrences
- **Value Objects**: Immutable objects defined by their values
- **Entities**: Objects with identity that persists over time

### Clean Architecture
- **Independence**: Domain layer has no external dependencies
- **Testability**: All components are easily testable
- **Framework Independence**: Not tied to any specific ORM or framework
- **Database Independence**: Can work with any data store

## Thread Safety

- **ValueObject**: Immutable and thread-safe
- **DomainEvent**: Immutable record type, thread-safe
- **Entity**: Identity is immutable, state changes should be controlled
- **AggregateRoot**: Domain events collection uses internal list with controlled access

## ORM Compatibility

All base classes include:
- Parameterless protected constructors for ORM support
- Protected setters where needed for ORM hydration
- Proper encapsulation with private setters for domain logic

Compatible with:
- Entity Framework Core
- Dapper
- NHibernate
- Other ORMs supporting parameterless constructors

## Best Practices

1. **Always validate in constructors** - Ensure entities are always in a valid state
2. **Use private setters** - Encapsulate state changes through methods
3. **Raise domain events** - Capture significant state changes
4. **Check business rules** - Use BusinessRuleValidator for rule enforcement
5. **Return Results** - Use Result pattern for operations that can fail
6. **Immutable value objects** - Never allow value objects to change
7. **Aggregate boundaries** - Keep aggregates small and focused

## Examples

### Complete Aggregate Example

```csharp
public class Order : AuditableAggregateRoot<Guid>
{
    private readonly List<OrderItem> _items = new();
    
    public string OrderNumber { get; private set; }
    public OrderStatus Status { get; private set; }
    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();
    
    private Order() : base() { }
    
    public Order(Guid id, string orderNumber) : base(id)
    {
        OrderNumber = orderNumber;
        Status = OrderStatus.Pending;
        SetCreatedAudit("system");
        RaiseDomainEvent(new OrderCreatedEvent(Id, orderNumber));
    }
    
    public Result AddItem(Product product, int quantity)
    {
        BusinessRuleValidator.CheckRule(new MinimumQuantityRule(quantity, 1));
        
        var item = new OrderItem(Guid.NewGuid(), product.Id, quantity, product.Price);
        _items.Add(item);
        
        RaiseDomainEvent(new OrderItemAddedEvent(Id, item.Id));
        return Result.Success();
    }
    
    public Result Complete()
    {
        if (Status != OrderStatus.Pending)
            return Result.Failure("Only pending orders can be completed");
        
        if (!_items.Any())
            return Result.Failure("Cannot complete order without items");
        
        Status = OrderStatus.Completed;
        SetUpdatedAudit("system");
        RaiseDomainEvent(new OrderCompletedEvent(Id));
        
        return Result.Success();
    }
}
```

## License

This library is part of the Deliris project.
