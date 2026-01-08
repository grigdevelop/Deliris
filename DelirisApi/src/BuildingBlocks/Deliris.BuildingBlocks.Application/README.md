# Deliris.BuildingBlocks.Application

Application layer building blocks for the Deliris platform implementing CQRS, validation, and cross-cutting concerns.

## Overview

This library provides reusable application layer components following Clean Architecture and CQRS patterns. It includes abstractions for commands, queries, validation, mapping, caching, and pipeline behaviors.

## Features

- ✅ **CQRS Pattern** - Command and Query separation with MediatR
- ✅ **Validation** - FluentValidation integration with pipeline behavior
- ✅ **Logging** - Automatic request/response logging
- ✅ **Performance Monitoring** - Tracks and logs slow requests
- ✅ **Transaction Management** - Automatic transaction handling for commands
- ✅ **Object Mapping** - AutoMapper integration with conventions
- ✅ **Caching** - Abstraction for distributed caching
- ✅ **Result Pattern** - Functional error handling extensions
- ✅ **Pagination** - Built-in pagination support

## Installation

```bash
dotnet add package Deliris.BuildingBlocks.Application
```

## Dependencies

- MediatR 12.4.1
- FluentValidation 11.10.0
- AutoMapper 13.0.1
- Microsoft.Extensions.Logging.Abstractions 8.0.2
- Microsoft.Extensions.Caching.Abstractions 8.0.0

## Quick Start

### 1. Register Services

```csharp
using Deliris.BuildingBlocks.Application.Extensions;

// In Program.cs or Startup.cs
services.AddApplicationServices(Assembly.GetExecutingAssembly());

// With transaction behavior
services.AddApplicationServices(Assembly.GetExecutingAssembly(), includeTransactionBehavior: true);
```

### 2. Create a Command

```csharp
using Deliris.BuildingBlocks.Application.Abstractions;

public record CreateProductCommand(string Name, decimal Price) : ICommand<Result<Guid>>;
```

### 3. Create a Command Handler

```csharp
public class CreateProductCommandHandler : ICommandHandler<CreateProductCommand, Result<Guid>>
{
    private readonly IRepository<Product, Guid> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateProductCommandHandler(
        IRepository<Product, Guid> repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(
        CreateProductCommand command,
        CancellationToken cancellationToken)
    {
        var product = new Product(Guid.NewGuid(), command.Name, command.Price);
        
        await _repository.AddAsync(product, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result.Success(product.Id);
    }
}
```

### 4. Create a Validator

```csharp
public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);
            
        RuleFor(x => x.Price)
            .GreaterThan(0);
    }
}
```

### 5. Create a Query

```csharp
public record GetProductByIdQuery(Guid Id) : IQuery<Result<ProductDto>>;
```

### 6. Create a Query Handler

```csharp
public class GetProductByIdQueryHandler : IQueryHandler<GetProductByIdQuery, Result<ProductDto>>
{
    private readonly IReadRepository<Product, Guid> _repository;
    private readonly IMapper _mapper;

    public GetProductByIdQueryHandler(
        IReadRepository<Product, Guid> repository,
        IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Result<ProductDto>> Handle(
        GetProductByIdQuery query,
        CancellationToken cancellationToken)
    {
        var product = await _repository.GetByIdAsync(query.Id, cancellationToken);
        
        if (product is null)
            return Result.Failure<ProductDto>("Product not found");
            
        var dto = _mapper.Map<ProductDto>(product);
        return Result.Success(dto);
    }
}
```

### 7. Execute Commands and Queries

```csharp
public class ProductsController : ControllerBase
{
    private readonly ISender _sender;

    public ProductsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateProductCommand command)
    {
        var result = await _sender.Send(command);
        
        return result.IsSuccess
            ? Ok(result.Value)
            : BadRequest(result.Error);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _sender.Send(new GetProductByIdQuery(id));
        
        return result.IsSuccess
            ? Ok(result.Value)
            : NotFound(result.Error);
    }
}
```

## Pagination

### Paginated Query

```csharp
public record GetProductsQuery : PaginatedQuery<Result<PaginatedResult<ProductDto>>>
{
    public string? Category { get; init; }
}
```

### Paginated Query Handler

```csharp
public class GetProductsQueryHandler : IQueryHandler<GetProductsQuery, Result<PaginatedResult<ProductDto>>>
{
    private readonly IReadRepository<Product, Guid> _repository;
    private readonly IMapper _mapper;

    public async Task<Result<PaginatedResult<ProductDto>>> Handle(
        GetProductsQuery query,
        CancellationToken cancellationToken)
    {
        var specification = new ProductsSpecification(query.Category);
        var products = await _repository.FindAsync(specification, cancellationToken);
        
        var pagedList = PagedList<Product>.Create(
            products,
            query.PageNumber,
            query.PageSize);
            
        var dtos = _mapper.Map<List<ProductDto>>(pagedList.Items);
        var result = new PaginatedResult<ProductDto>(
            dtos,
            pagedList.PageNumber,
            pagedList.PageSize,
            pagedList.TotalCount);
            
        return Result.Success(result);
    }
}
```

## Mapping

### Using IMapFrom

```csharp
public class ProductDto : IMapFrom<Product>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
}
```

### Custom Mapping

```csharp
public class ProductDto : IMapFrom<Product>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string DisplayName { get; set; } = string.Empty;

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Product, ProductDto>()
            .ForMember(d => d.DisplayName, opt => opt.MapFrom(s => $"{s.Name} - ${s.Price}"));
    }
}
```

## Caching

### Using ICacheService

```csharp
public class GetProductByIdQueryHandler : IQueryHandler<GetProductByIdQuery, Result<ProductDto>>
{
    private readonly IReadRepository<Product, Guid> _repository;
    private readonly IMapper _mapper;
    private readonly ICacheService _cache;

    public async Task<Result<ProductDto>> Handle(
        GetProductByIdQuery query,
        CancellationToken cancellationToken)
    {
        var cacheKey = $"product:{query.Id}";
        
        var cached = await _cache.GetAsync<ProductDto>(cacheKey, cancellationToken);
        if (cached is not null)
            return Result.Success(cached);
            
        var product = await _repository.GetByIdAsync(query.Id, cancellationToken);
        
        if (product is null)
            return Result.Failure<ProductDto>("Product not found");
            
        var dto = _mapper.Map<ProductDto>(product);
        
        await _cache.SetAsync(
            cacheKey,
            dto,
            CacheOptions.WithSlidingExpiration(TimeSpan.FromMinutes(10)),
            cancellationToken);
            
        return Result.Success(dto);
    }
}
```

## Result Pattern Extensions

```csharp
var result = await _sender.Send(new CreateProductCommand("Product", 99.99m));

// Map
var mappedResult = result.Map(id => new { ProductId = id });

// Bind
var boundResult = result.Bind(id => GetProductById(id));

// Match
var response = result.Match(
    onSuccess: id => Ok(id),
    onFailure: error => BadRequest(error));

// OnSuccess/OnFailure
result
    .OnSuccess(id => _logger.LogInformation("Created product {Id}", id))
    .OnFailure(error => _logger.LogError("Failed to create product: {Error}", error));
```

## Pipeline Behaviors

The following behaviors are automatically applied to all requests:

### 1. ValidationBehavior
- Validates requests using FluentValidation
- Throws `ValidationException` if validation fails
- Runs before the handler

### 2. LoggingBehavior
- Logs request and response information
- Logs errors with stack traces
- Useful for debugging and monitoring

### 3. PerformanceBehavior
- Measures request execution time
- Logs warning for slow requests (>500ms)
- Helps identify performance bottlenecks

### 4. TransactionBehavior (Optional)
- Wraps commands in database transactions
- Automatically commits on success
- Rolls back on exceptions
- Only applies to commands (not queries)

## Validation Extensions

```csharp
public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty(); // Custom GUID validation
            
        RuleFor(x => x.Email)
            .IsValidEmail(); // Email validation
            
        RuleFor(x => x.Items)
            .NotEmpty(); // Collection validation
            
        RuleFor(x => x.Price)
            .InRange(0.01m, 10000m); // Range validation
    }
}
```

## Exception Handling

### Application Exceptions

```csharp
// Validation exception
throw new ValidationException("PropertyName", "Error message");

// Unauthorized exception
throw new UnauthorizedException("You must be logged in");

// Forbidden exception
throw new ForbiddenException("You don't have permission");

// Custom application exception
public class ProductNotFoundException : ApplicationException
{
    public ProductNotFoundException(Guid id)
        : base($"Product with ID {id} was not found", "PRODUCT_NOT_FOUND")
    {
    }
}
```

## Best Practices

### 1. Command Naming
- Use imperative verbs: `CreateProduct`, `UpdateProduct`, `DeleteProduct`
- Be specific: `ActivateUserAccount` vs `UpdateUser`

### 2. Query Naming
- Use descriptive names: `GetProductById`, `GetActiveProducts`
- Include filters in name: `GetProductsByCategory`

### 3. Validation
- Always validate commands
- Queries typically don't need validation (except pagination parameters)
- Use custom validators for complex rules

### 4. Result Pattern
- Return `Result` or `Result<T>` from handlers
- Use `Result.Success()` and `Result.Failure()` factory methods
- Leverage extension methods for functional composition

### 5. Transactions
- Enable `TransactionBehavior` only when needed
- Commands should modify state, queries should not
- Keep transactions short and focused

### 6. Caching
- Cache read-heavy data
- Use appropriate expiration strategies
- Invalidate cache on updates

### 7. Logging
- Let `LoggingBehavior` handle standard logging
- Add custom logging for business-specific events
- Use structured logging with proper context

## Architecture

```
Application Layer
├── Abstractions/           # CQRS interfaces
│   ├── ICommand.cs
│   ├── ICommandHandler.cs
│   ├── IQuery.cs
│   ├── IQueryHandler.cs
│   └── IApplicationService.cs
├── Behaviors/              # MediatR pipeline behaviors
│   ├── ValidationBehavior.cs
│   ├── LoggingBehavior.cs
│   ├── PerformanceBehavior.cs
│   └── TransactionBehavior.cs
├── Caching/                # Caching abstractions
│   ├── ICacheService.cs
│   └── CacheOptions.cs
├── Common/                 # Base classes and DTOs
│   ├── BaseCommand.cs
│   ├── BaseQuery.cs
│   ├── PaginatedQuery.cs
│   └── PaginatedResult.cs
├── Exceptions/             # Application exceptions
│   ├── ApplicationException.cs
│   ├── ValidationException.cs
│   ├── UnauthorizedException.cs
│   └── ForbiddenException.cs
├── Extensions/             # Extension methods
│   ├── ResultExtensions.cs
│   └── ServiceCollectionExtensions.cs
├── Mapping/                # AutoMapper support
│   ├── IMapFrom.cs
│   ├── IMapTo.cs
│   └── MappingProfile.cs
└── Validation/             # Validation extensions
    └── AbstractValidatorExtensions.cs
```

## Testing

### Unit Testing Commands

```csharp
[Fact]
public async Task Handle_ValidCommand_ReturnsSuccess()
{
    // Arrange
    var repository = new Mock<IRepository<Product, Guid>>();
    var unitOfWork = new Mock<IUnitOfWork>();
    var handler = new CreateProductCommandHandler(repository.Object, unitOfWork.Object);
    var command = new CreateProductCommand("Product", 99.99m);

    // Act
    var result = await handler.Handle(command, CancellationToken.None);

    // Assert
    Assert.True(result.IsSuccess);
    repository.Verify(r => r.AddAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()), Times.Once);
}
```

### Testing Validators

```csharp
[Fact]
public void Validate_EmptyName_ReturnsError()
{
    // Arrange
    var validator = new CreateProductCommandValidator();
    var command = new CreateProductCommand("", 99.99m);

    // Act
    var result = validator.Validate(command);

    // Assert
    Assert.False(result.IsValid);
    Assert.Contains(result.Errors, e => e.PropertyName == nameof(command.Name));
}
```

## Contributing

When adding new features to this library:

1. Follow existing patterns and conventions
2. Add comprehensive XML documentation
3. Include unit tests
4. Update this README
5. Consider backward compatibility

## License

This library is part of the Deliris platform and follows the project's licensing terms.

## Related Projects

- `Deliris.BuildingBlocks.Domain` - Domain layer building blocks
- `Deliris.BuildingBlocks.Domain.Abstractions` - Domain abstractions
- `Deliris.BuildingBlocks.Infrastructure` - Infrastructure implementations

## Support

For issues, questions, or contributions, please refer to the main Deliris project documentation.
