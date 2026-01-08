The following projects will be maintained within a single monorepo:

- **Backend Services**: .NET microservices architecture
- **Admin Dashboard**: Next.js web application for platform administration
- **Partner Web App**: Next.js frontend for delivery partners and vendors
- **Customer Web App**: Next.js frontend for end-user ordering
- **Partner Mobile App**: Native/cross-platform mobile application for delivery partners
- **Customer Mobile App**: Native/cross-platform mobile application for customers

create md file with detailed instructions of each project structure and libraries used

- .net 10
- c# 14
- instead of identityserver4 - openiddict
- asp.net core 10
- mediatr version 14.0.0
- automapper version 16.0.0
- fluentvalidation version 12.1.1
- entityframeworkcore version 10.0.1
- Npgsql.EntityFrameworkCore.PostgreSQL version 10.0.0
- dapper version 2.1.66
- Swashbuckle.AspNetCore.Swagger version 10.1.0
- signalr version 10.0.1
- refit version 9.0.2
- MassTransit version 8.5.7
- RabbitMQ.Client version 7.2.0
- openiddict version 7.2.0
- xunit version 2.9.3
- moq version 4.20.72
- FluentAssertions version 8.8.0
- Serilog version 4.3.0
- Polly version 8.6.5


## IdentityDbContext Related Classes

### Core DbContext
- **IdentityDbContext**: Main database context for identity management
  - Inherits from `DbContext`
  - Configures entity mappings for identity-related entities
  - Manages database connections and transactions

### Entity Classes
- **User**: Represents application users
- **Role**: Defines user roles and permissions
- **UserRole**: Junction table for many-to-many relationship between users and roles
- **UserClaim**: Stores custom claims associated with users
- **RoleClaim**: Stores claims associated with roles
- **UserLogin**: Manages external login providers (Google, Facebook, etc.)
- **UserToken**: Stores authentication tokens for users

### Configuration Classes
- **UserConfiguration**: Entity type configuration for User entity
- **RoleConfiguration**: Entity type configuration for Role entity
- **UserRoleConfiguration**: Entity type configuration for UserRole entity
- **UserClaimConfiguration**: Entity type configuration for UserClaim entity
- **RoleClaimConfiguration**: Entity type configuration for RoleClaim entity
- **UserLoginConfiguration**: Entity type configuration for UserLogin entity
- **UserTokenConfiguration**: Entity type configuration for UserToken entity

### Repository Interfaces
- **IUserRepository**: Repository interface for user operations
- **IRoleRepository**: Repository interface for role operations
- **IUserClaimRepository**: Repository interface for user claim operations
- **IRoleClaimRepository**: Repository interface for role claim operations

### Repository Implementations
- **UserRepository**: Implements IUserRepository using Dapper and EF Core
- **RoleRepository**: Implements IRoleRepository using Dapper and EF Core
- **UserClaimRepository**: Implements IUserClaimRepository
- **RoleClaimRepository**: Implements IRoleClaimRepository

### Unit of Work
- **IIdentityUnitOfWork**: Unit of work interface for identity operations
- **IdentityUnitOfWork**: Implementation of unit of work pattern for identity context

In my identity service project I need to create entity classes of identityDbContext. Should I use Deliris.BuildingBlocks.Domain classes in implementation or not. I need to add user registration endpoint which will trigger event for message broker.

## Identity Service Implementation Requirements

### Entity Design Question
I'm implementing the Identity Service's IdentityDbContext and need architectural guidance:

**Question**: Should the Identity Service entity classes (User, Role, UserRole, etc.) inherit from or utilize the base classes in `Deliris.BuildingBlocks.Domain`?

**Context**:
- The BuildingBlocks.Domain project likely contains shared base entities (e.g., `Entity`, `AggregateRoot`, `ValueObject`)
- Identity entities have specific requirements for ASP.NET Core Identity integration
- Need to maintain consistency across microservices while respecting bounded context boundaries

**Considerations**:
- Domain-driven design principles and bounded contexts
- Shared kernel vs. separate contexts trade-offs
- Impact on maintainability and coupling between services

### Feature Requirements

**User Registration Endpoint**:
- Create a POST endpoint for user registration
- Implement domain event publishing for message broker integration
- Event should be triggered upon successful user registration
- Follow CQRS pattern if applicable

**Technical Requirements**:
- Ensure proper validation and error handling
- Implement idempotency for registration requests
- Follow microservice communication patterns (async messaging)
- Maintain transactional consistency between user creation and event publishing

**Questions to Address**:
1. Should identity entities extend BuildingBlocks base classes or remain independent?
2. What domain events should be published during user registration?
3. Which message broker pattern should be used (outbox pattern, direct publishing, etc.)?
4. How to handle distributed transaction concerns between database writes and message publishing?

There is Deliris.BuildingBlocks.Domain.Abstractions project, empty class library. Also I referenced it in Deliris.BuildingBlocks.Domain project. Implement abstractions in this project and use them in Deliris.BuildingBlocks.Domain. If there is already exists interfaces in Deliris.BuildingBlocks.Domain project, move them to Deliris.BuildingBlocks.Domain.Abstractions project.

In my Identity Service project, I'm planning to use IdentityDbContext and entity classes such as IdentityUser, IdentityRole, IdentityUserRole, IdentityUserClaim, IdentityRoleClaim, IdentityUserLogin, and IdentityUserToken. Should I use Deliris.BuildingBlocks.Domain.Abstractions in the implementation or not?

### Message Broker Technologies

**RabbitMQ**:
- Mature and reliable AMQP-based message broker
- Already included in project dependencies (RabbitMQ.Client version 7.2.0)
- Excellent integration with MassTransit (version 8.5.7)
- Supports various messaging patterns (pub/sub, request/response, routing)
- Built-in management UI and monitoring
- Strong community support and extensive documentation

**Azure Service Bus**:
- Enterprise-grade cloud messaging service
- Native integration with Azure ecosystem
- Advanced features: message sessions, dead-letter queues, scheduled messages
- Automatic scaling and high availability
- Pay-as-you-go pricing model

**Apache Kafka**:
- High-throughput distributed streaming platform
- Excellent for event sourcing and event-driven architectures
- Durable message storage with replay capabilities
- Strong ordering guarantees within partitions
- Best for high-volume, real-time data processing

**Redis Pub/Sub**:
- Lightweight and fast in-memory messaging
- Simple pub/sub pattern implementation
- Good for real-time notifications and caching
- Limited durability and persistence options
- Best for ephemeral, low-latency messaging

**Amazon SQS/SNS**:
- AWS-native messaging services
- SQS for queuing, SNS for pub/sub
- Fully managed with automatic scaling
- Integration with AWS ecosystem
- Serverless-friendly architecture

**NATS**:
- Lightweight, high-performance messaging system
- Cloud-native and Kubernetes-friendly
- Simple deployment and configuration
- Good for microservices communication
- Lower operational complexity

**Recommendation for Deliris Project**:
Given the existing dependencies (MassTransit 8.5.7 and RabbitMQ.Client 7.2.0), **RabbitMQ with MassTransit** is the recommended choice because:
- Already included in the technology stack
- MassTransit provides excellent abstraction layer
- Proven reliability for microservices architectures
- Supports outbox pattern for transactional messaging
- Easy local development and testing
- Production-ready with minimal configuration

### Message Durability and Reliability with RabbitMQ and MassTransit

**Short Answer**: No, you won't lose messages if properly configured.

**Message Durability Guarantees**:

1. **Persistent Messages**:
   - MassTransit marks messages as persistent by default
   - Messages are written to disk before acknowledgment
   - Survives broker restarts

2. **Publisher Confirms**:
   - RabbitMQ confirms message receipt before returning
   - MassTransit waits for confirmation by default
   - Ensures messages are safely stored

3. **Consumer Acknowledgments**:
   - Messages remain in queue until explicitly acknowledged
   - Automatic redelivery on consumer failure
   - Configurable retry policies

4. **Outbox Pattern Support**:
   - MassTransit supports transactional outbox pattern
   - Ensures exactly-once message publishing
   - Atomic database + message operations

**Configuration Best Practices**:

```csharp
services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("rabbitmq://localhost", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        // Enable publisher confirms
        cfg.PublisherConfirmation = true;

        // Configure retry policy
        cfg.UseMessageRetry(r => r.Incremental(3, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(2)));

        // Enable circuit breaker
        cfg.UseCircuitBreaker(cb =>
        {
            cb.TrackingPeriod = TimeSpan.FromMinutes(1);
            cb.TripThreshold = 15;
            cb.ActiveThreshold = 10;
        });

        // Durable queues and exchanges
        cfg.ConfigureEndpoints(context, new DefaultEndpointNameFormatter(false));
    });
});
```

**Potential Message Loss Scenarios (and Solutions)**:

1. **Broker Failure Without Persistence**:
   - ❌ Problem: Non-durable queues lose messages on restart
   - ✅ Solution: Use durable queues (MassTransit default)

2. **Consumer Processing Failure**:
   - ❌ Problem: Message acknowledged before processing completes
   - ✅ Solution: Use MassTransit's automatic acknowledgment after successful processing

3. **Network Partitions**:
   - ❌ Problem: Messages in-flight during network failure
   - ✅ Solution: Publisher confirms + consumer acknowledgments

4. **Database Transaction + Message Publishing**:
   - ❌ Problem: Database commits but message publishing fails
   - ✅ Solution: Implement Outbox Pattern

**Outbox Pattern Implementation**:

```csharp
// Entity for outbox
public class OutboxMessage : Entity<Guid>
{
    public string MessageType { get; set; }
    public string Payload { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ProcessedAt { get; set; }
    public string? Error { get; set; }
}

// Save message in same transaction
await using var transaction = await dbContext.Database.BeginTransactionAsync();
try
{
    // Save domain entity
    await dbContext.Users.AddAsync(user);
    
    // Save outbox message
    await dbContext.OutboxMessages.AddAsync(new OutboxMessage
    {
        MessageType = nameof(UserRegisteredEvent),
        Payload = JsonSerializer.Serialize(userRegisteredEvent),
        CreatedAt = DateTime.UtcNow
    });
    
    await dbContext.SaveChangesAsync();
    await transaction.CommitAsync();
}
catch
{
    await transaction.RollbackAsync();
    throw;
}

// Background service publishes outbox messages
public class OutboxProcessor : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var messages = await GetUnprocessedMessages();
            foreach (var message in messages)
            {
                try
                {
                    await _publishEndpoint.Publish(DeserializeMessage(message));
                    await MarkAsProcessed(message.Id);
                }
                catch (Exception ex)
                {
                    await MarkAsFailed(message.Id, ex.Message);
                }
            }
            await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
        }
    }
}
```

**Monitoring and Observability**:

```csharp
// Add Serilog for message tracking
services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.ConnectSendObserver(new SendObserver());
        cfg.ConnectConsumeObserver(new ConsumeObserver());
    });
});

public class SendObserver : ISendObserver
{
    public Task PreSend<T>(SendContext<T> context) where T : class
    {
        Log.Information("Sending message {MessageType} with ID {MessageId}", 
            typeof(T).Name, context.MessageId);
        return Task.CompletedTask;
    }

    public Task PostSend<T>(SendContext<T> context) where T : class
    {
        Log.Information("Message {MessageType} sent successfully", typeof(T).Name);
        return Task.CompletedTask;
    }

    public Task SendFault<T>(SendContext<T> context, Exception exception) where T : class
    {
        Log.Error(exception, "Failed to send message {MessageType}", typeof(T).Name);
        return Task.CompletedTask;
    }
}
```

**Recommendation for Identity Service**:
- Use **Outbox Pattern** for user registration events
- Enable **Publisher Confirms** and **Consumer Acknowledgments**
- Implement **Retry Policies** with exponential backoff
- Add **Dead Letter Queues** for failed messages
- Monitor message flow with **Serilog** and **Application Insights**

**Result**: With proper configuration, RabbitMQ + MassTransit provides reliable, exactly-once message delivery guarantees suitable for production microservices.

### Integration Testing with Testcontainers and RabbitMQ

**Testcontainers** provides lightweight, disposable Docker containers for integration testing, making it ideal for testing message broker interactions.

**Setup and Configuration**:

1. **Add Required NuGet Packages**:
```xml
<ItemGroup>
  <PackageReference Include="Testcontainers.RabbitMq" Version="3.10.0" />
  <PackageReference Include="xunit" Version="2.9.3" />
  <PackageReference Include="FluentAssertions" Version="8.8.0" />
  <PackageReference Include="MassTransit.TestFramework" Version="8.5.7" />
  <PackageReference Include="Microsoft.Extensions.DependencyInjection" Version="10.0.0" />
</ItemGroup>
```

2. **Test Fixture for RabbitMQ Container**:
```csharp
using Testcontainers.RabbitMq;
using Xunit;

public class RabbitMqTestFixture : IAsyncLifetime
{
    private readonly RabbitMqContainer _rabbitMqContainer;

    public RabbitMqTestFixture()
    {
        _rabbitMqContainer = new RabbitMqBuilder()
            .WithImage("rabbitmq:3.13-management")
            .WithPortBinding(5672, true)
            .WithPortBinding(15672, true)
            .WithUsername("guest")
            .WithPassword("guest")
            .WithCleanUp(true)
            .Build();
    }

    public string ConnectionString => _rabbitMqContainer.GetConnectionString();
    public int Port => _rabbitMqContainer.GetMappedPublicPort(5672);
    public string Hostname => _rabbitMqContainer.Hostname;

    public async Task InitializeAsync()
    {
        await _rabbitMqContainer.StartAsync();
        
        // Wait for RabbitMQ to be fully ready
        await Task.Delay(TimeSpan.FromSeconds(5));
    }

    public async Task DisposeAsync()
    {
        await _rabbitMqContainer.DisposeAsync();
    }
}
```

3. **Integration Test Class**:
```csharp
using MassTransit;
using MassTransit.Testing;
using Microsoft.Extensions.DependencyInjection;
using FluentAssertions;
using Xunit;

public class UserRegistrationIntegrationTests : IClassFixture<RabbitMqTestFixture>
{
    private readonly RabbitMqTestFixture _fixture;

    public UserRegistrationIntegrationTests(RabbitMqTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task Should_Publish_UserRegisteredEvent_Successfully()
    {
        // Arrange
        await using var provider = new ServiceCollection()
            .AddMassTransitTestHarness(x =>
            {
                x.AddConsumer<UserRegisteredEventConsumer>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(_fixture.Hostname, _fixture.Port, "/", h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });

                    cfg.ConfigureEndpoints(context);
                });
            })
            .BuildServiceProvider(true);

        var harness = provider.GetRequiredService<ITestHarness>();
        await harness.Start();

        try
        {
            var userId = Guid.NewGuid();
            var userEmail = "test@example.com";

            // Act
            await harness.Bus.Publish(new UserRegisteredEvent(userId, userEmail));

            // Assert
            (await harness.Published.Any<UserRegisteredEvent>()).Should().BeTrue();
            (await harness.Consumed.Any<UserRegisteredEvent>()).Should().BeTrue();

            var consumerHarness = harness.GetConsumerHarness<UserRegisteredEventConsumer>();
            (await consumerHarness.Consumed.Any<UserRegisteredEvent>()).Should().BeTrue();

            var message = (await consumerHarness.Consumed.SelectAsync<UserRegisteredEvent>().First()).Context.Message;
            message.UserId.Should().Be(userId);
            message.Email.Should().Be(userEmail);
        }
        finally
        {
            await harness.Stop();
        }
    }

    [Fact]
    public async Task Should_Handle_Message_Retry_On_Failure()
    {
        // Arrange
        var attemptCount = 0;
        await using var provider = new ServiceCollection()
            .AddSingleton<AttemptCounter>(new AttemptCounter())
            .AddMassTransitTestHarness(x =>
            {
                x.AddConsumer<FailingConsumer>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(_fixture.Hostname, _fixture.Port, "/", h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });

                    cfg.UseMessageRetry(r => r.Immediate(2));
                    cfg.ConfigureEndpoints(context);
                });
            })
            .BuildServiceProvider(true);

        var harness = provider.GetRequiredService<ITestHarness>();
        await harness.Start();

        try
        {
            // Act
            await harness.Bus.Publish(new TestMessage { Value = "test" });

            // Wait for retries
            await Task.Delay(TimeSpan.FromSeconds(3));

            // Assert
            var consumerHarness = harness.GetConsumerHarness<FailingConsumer>();
            var counter = provider.GetRequiredService<AttemptCounter>();
            
            counter.Count.Should().Be(3); // Initial attempt + 2 retries
        }
        finally
        {
            await harness.Stop();
        }
    }

    [Fact]
    public async Task Should_Route_Messages_To_Multiple_Consumers()
    {
        // Arrange
        await using var provider = new ServiceCollection()
            .AddMassTransitTestHarness(x =>
            {
                x.AddConsumer<EmailNotificationConsumer>();
                x.AddConsumer<AuditLogConsumer>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(_fixture.Hostname, _fixture.Port, "/", h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });

                    cfg.ConfigureEndpoints(context);
                });
            })
            .BuildServiceProvider(true);

        var harness = provider.GetRequiredService<ITestHarness>();
        await harness.Start();

        try
        {
            var userId = Guid.NewGuid();

            // Act
            await harness.Bus.Publish(new UserRegisteredEvent(userId, "test@example.com"));

            // Assert
            var emailConsumerHarness = harness.GetConsumerHarness<EmailNotificationConsumer>();
            var auditConsumerHarness = harness.GetConsumerHarness<AuditLogConsumer>();

            (await emailConsumerHarness.Consumed.Any<UserRegisteredEvent>()).Should().BeTrue();
            (await auditConsumerHarness.Consumed.Any<UserRegisteredEvent>()).Should().BeTrue();
        }
        finally
        {
            await harness.Stop();
        }
    }

    [Fact]
    public async Task Should_Handle_Dead_Letter_Queue_For_Failed_Messages()
    {
        // Arrange
        await using var provider = new ServiceCollection()
            .AddMassTransitTestHarness(x =>
            {
                x.AddConsumer<AlwaysFailingConsumer>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(_fixture.Hostname, _fixture.Port, "/", h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });

                    cfg.UseMessageRetry(r => r.Immediate(1));
                    cfg.ConfigureEndpoints(context);
                });
            })
            .BuildServiceProvider(true);

        var harness = provider.GetRequiredService<ITestHarness>();
        await harness.Start();

        try
        {
            // Act
            await harness.Bus.Publish(new TestMessage { Value = "fail" });

            // Wait for processing
            await Task.Delay(TimeSpan.FromSeconds(2));

            // Assert
            var consumerHarness = harness.GetConsumerHarness<AlwaysFailingConsumer>();
            (await consumerHarness.Consumed.Any<TestMessage>(x => x.Context.Message.Value == "fail"))
                .Should().BeTrue();
        }
        finally
        {
            await harness.Stop();
        }
    }
}

// Test Consumers
public class UserRegisteredEventConsumer : IConsumer<UserRegisteredEvent>
{
    public Task Consume(ConsumeContext<UserRegisteredEvent> context)
    {
        // Simulate processing
        return Task.CompletedTask;
    }
}

public class FailingConsumer : IConsumer<TestMessage>
{
    private readonly AttemptCounter _counter;

    public FailingConsumer(AttemptCounter counter)
    {
        _counter = counter;
    }

    public Task Consume(ConsumeContext<TestMessage> context)
    {
        _counter.Increment();
        throw new InvalidOperationException("Simulated failure");
    }
}

public class AlwaysFailingConsumer : IConsumer<TestMessage>
{
    public Task Consume(ConsumeContext<TestMessage> context)
    {
        throw new InvalidOperationException("Always fails");
    }
}

public class EmailNotificationConsumer : IConsumer<UserRegisteredEvent>
{
    public Task Consume(ConsumeContext<UserRegisteredEvent> context)
    {
        // Send email notification
        return Task.CompletedTask;
    }
}

public class AuditLogConsumer : IConsumer<UserRegisteredEvent>
{
    public Task Consume(ConsumeContext<UserRegisteredEvent> context)
    {
        // Log audit event
        return Task.CompletedTask;
    }
}

// Test Messages
public record TestMessage
{
    public string Value { get; init; } = string.Empty;
}

// Helper Classes
public class AttemptCounter
{
    private int _count;
    public int Count => _count;
    public void Increment() => Interlocked.Increment(ref _count);
}
```

4. **Testing Outbox Pattern**:
```csharp
public class OutboxPatternIntegrationTests : IClassFixture<RabbitMqTestFixture>
{
    private readonly RabbitMqTestFixture _fixture;

    public OutboxPatternIntegrationTests(RabbitMqTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task Should_Publish_Outbox_Messages_Successfully()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<IdentityDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        await using var dbContext = new IdentityDbContext(options);
        await dbContext.Database.EnsureCreatedAsync();

        await using var provider = new ServiceCollection()
            .AddSingleton(dbContext)
            .AddMassTransitTestHarness(x =>
            {
                x.AddConsumer<UserRegisteredEventConsumer>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(_fixture.Hostname, _fixture.Port, "/", h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });

                    cfg.ConfigureEndpoints(context);
                });
            })
            .BuildServiceProvider(true);

        var harness = provider.GetRequiredService<ITestHarness>();
        await harness.Start();

        try
        {
            // Act - Save to outbox
            var userId = Guid.NewGuid();
            var outboxMessage = new OutboxMessage
            {
                Id = Guid.NewGuid(),
                MessageType = nameof(UserRegisteredEvent),
                Payload = JsonSerializer.Serialize(new UserRegisteredEvent(userId, "test@example.com")),
                CreatedAt = DateTime.UtcNow
            };

            await dbContext.OutboxMessages.AddAsync(outboxMessage);
            await dbContext.SaveChangesAsync();

            // Simulate outbox processor
            var message = JsonSerializer.Deserialize<UserRegisteredEvent>(outboxMessage.Payload);
            await harness.Bus.Publish(message!);

            // Assert
            (await harness.Published.Any<UserRegisteredEvent>()).Should().BeTrue();
            (await harness.Consumed.Any<UserRegisteredEvent>()).Should().BeTrue();
        }
        finally
        {
            await harness.Stop();
        }
    }
}
```

**Best Practices**:

1. **Use Test Harness**: MassTransit's test harness provides better control and observability
2. **Container Lifecycle**: Share container across tests in a class with `IClassFixture`
3. **Cleanup**: Always dispose containers to free resources
4. **Wait Strategies**: Add delays for async operations to complete
5. **Isolation**: Use unique queue names or separate containers for parallel tests
6. **Assertions**: Verify both publishing and consumption of messages
7. **Error Scenarios**: Test retry policies, dead letter queues, and failures

How to implement the registration endpoint of the Identity Service with RabbitMQ message broker?

## Implementing User Registration Endpoint with RabbitMQ Integration

### Requirements
I need to implement a complete user registration endpoint in the Identity Service that:

1. **Endpoint Specifications**:
   - POST `/api/identity/register`
   - Accepts user registration data (email, password, username, etc.)
   - Validates input using FluentValidation
   - Returns appropriate HTTP status codes (201 Created, 400 Bad Request, 409 Conflict)

2. **Domain Layer**:
   - User entity inheriting from `AuditableAggregateRoot<Guid>`
   - Domain event: `UserRegisteredEvent` to be raised upon successful registration
   - Use Result pattern for operation outcomes

3. **Database Operations**:
   - Save user to IdentityDbContext using Entity Framework Core
   - Implement repository pattern with `IUserRepository`
   - Use Unit of Work pattern for transaction management

4. **Message Broker Integration**:
   - Publish `UserRegisteredEvent` to RabbitMQ using MassTransit
   - Implement Outbox Pattern for reliable message delivery
   - Ensure transactional consistency between database write and message publishing
   - Configure publisher confirms and retry policies

5. **Architecture Patterns**:
   - Follow CQRS with MediatR (command: `RegisterUserCommand`)
   - Implement proper error handling and logging with Serilog
   - Use AutoMapper for DTO mapping
   - Apply idempotency to prevent duplicate registrations

### Technical Stack
- .NET 10, C# 14
- Entity Framework Core 10.0.1 with PostgreSQL
- MassTransit 8.5.7 + RabbitMQ.Client 7.2.0
- MediatR 14.0.0
- FluentValidation 12.1.1
- AutoMapper 16.0.0

### Expected Implementation
Please provide complete code examples for:
- User entity with domain events
- Registration command and handler
- API controller endpoint
- Repository implementation
- MassTransit configuration with Outbox Pattern
- Integration test using Testcontainers

## Do I need OpenIddict for User Registration?

**Short Answer**: No, you don't need OpenIddict for implementing user registration.

**Explanation**:

OpenIddict is an authentication/authorization framework that implements OAuth 2.0 and OpenID Connect protocols. It's used for:
- Token generation and validation
- User authentication flows
- Authorization code flows
- Client credentials flows
- Single Sign-On (SSO)

**User Registration is separate from authentication/authorization**:
- Registration is a business logic operation that creates a new user account
- It involves validating user input, creating user entity, storing in database, and publishing domain events
- Authentication (handled by OpenIddict) comes *after* registration

**When you DO need OpenIddict**:
- Implementing login/logout endpoints
- Generating JWT/access tokens
- Validating authentication tokens
- Implementing OAuth2/OIDC flows
- Protecting API endpoints with token validation

**For User Registration, you only need**:
- Entity Framework Core (database operations)
- MediatR (CQRS pattern)
- FluentValidation (input validation)
- MassTransit + RabbitMQ (event publishing)
- AutoMapper (DTO mapping)

**Typical Flow**:
1. User registers → Creates user account (no OpenIddict needed)
2. User logs in → OpenIddict generates tokens
3. User accesses protected resources → OpenIddict validates tokens

**Recommendation**: Implement user registration without OpenIddict. Add OpenIddict integration later when implementing authentication endpoints.
