using System.Reflection;

namespace Deliris.BuildingBlocks.Application.Mapping;

/// <summary>
/// Base AutoMapper profile that automatically configures mappings from IMapFrom and IMapTo interfaces.
/// </summary>
public class MappingProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MappingProfile"/> class.
    /// </summary>
    /// <param name="assembly">The assembly to scan for mapping configurations.</param>
    public MappingProfile(Assembly assembly)
    {
        ApplyMappingsFromAssembly(assembly);
    }

    private void ApplyMappingsFromAssembly(Assembly assembly)
    {
        var mapFromType = typeof(IMapFrom<>);
        var mapToType = typeof(IMapTo<>);

        var types = assembly.GetExportedTypes()
            .Where(t => t is { IsClass: true, IsAbstract: false })
            .ToList();

        foreach (var type in types)
        {
            // Handle IMapFrom<T>
            var mapFromInterfaces = type.GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == mapFromType);

            foreach (var mapFromInterface in mapFromInterfaces)
            {
                var instance = Activator.CreateInstance(type);
                var methodInfo = mapFromInterface.GetMethod("Mapping") 
                    ?? mapFromInterface.GetMethod("Deliris.BuildingBlocks.Application.Mapping.IMapFrom<T>.Mapping");

                methodInfo?.Invoke(instance, [this]);
            }

            // Handle IMapTo<T>
            var mapToInterfaces = type.GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == mapToType);

            foreach (var mapToInterface in mapToInterfaces)
            {
                var instance = Activator.CreateInstance(type);
                var methodInfo = mapToInterface.GetMethod("Mapping")
                    ?? mapToInterface.GetMethod("Deliris.BuildingBlocks.Application.Mapping.IMapTo<T>.Mapping");

                methodInfo?.Invoke(instance, [this]);
            }
        }
    }
}
