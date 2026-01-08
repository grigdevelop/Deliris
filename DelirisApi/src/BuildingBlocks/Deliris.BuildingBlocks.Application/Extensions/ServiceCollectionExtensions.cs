using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Deliris.BuildingBlocks.Application.Extensions;

/// <summary>
/// Extension methods for configuring application services.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds application layer services to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="assembly">The assembly containing handlers and validators.</param>
    /// <returns>The service collection.</returns>
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services,
        Assembly assembly)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(assembly);
            cfg.AddOpenBehavior(typeof(Behaviors.ValidationBehavior<,>));
            cfg.AddOpenBehavior(typeof(Behaviors.LoggingBehavior<,>));
            cfg.AddOpenBehavior(typeof(Behaviors.PerformanceBehavior<,>));
        });

        services.AddValidatorsFromAssembly(assembly);

        services.AddAutoMapper(_ => { },assembly);

        return services;
    }

    /// <summary>
    /// Adds application layer services with custom configuration.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="assembly">The assembly containing handlers and validators.</param>
    /// <param name="includeTransactionBehavior">Whether to include transaction behavior.</param>
    /// <returns>The service collection.</returns>
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services,
        Assembly assembly,
        bool includeTransactionBehavior)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(assembly);
            cfg.AddOpenBehavior(typeof(Behaviors.ValidationBehavior<,>));
            cfg.AddOpenBehavior(typeof(Behaviors.LoggingBehavior<,>));
            cfg.AddOpenBehavior(typeof(Behaviors.PerformanceBehavior<,>));
            
            if (includeTransactionBehavior)
            {
                cfg.AddOpenBehavior(typeof(Behaviors.TransactionBehavior<,>));
            }
        });

        services.AddValidatorsFromAssembly(assembly);

        services.AddAutoMapper(_ => { },assembly);

        return services;
    }

    /// <summary>
    /// Adds application layer services from multiple assemblies.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="assemblies">The assemblies containing handlers and validators.</param>
    /// <returns>The service collection.</returns>
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services,
        params Assembly[] assemblies)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies(assemblies);
            cfg.AddOpenBehavior(typeof(Behaviors.ValidationBehavior<,>));
            cfg.AddOpenBehavior(typeof(Behaviors.LoggingBehavior<,>));
            cfg.AddOpenBehavior(typeof(Behaviors.PerformanceBehavior<,>));
        });

        foreach (var assembly in assemblies)
        {
            services.AddValidatorsFromAssembly(assembly);
            services.AddAutoMapper(_ => { },assembly);
        }

        return services;
    }
}
