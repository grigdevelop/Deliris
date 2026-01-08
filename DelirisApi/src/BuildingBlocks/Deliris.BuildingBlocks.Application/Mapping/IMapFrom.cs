namespace Deliris.BuildingBlocks.Application.Mapping;

/// <summary>
/// Interface for objects that can be mapped from a source type.
/// </summary>
/// <typeparam name="T">The source type to map from.</typeparam>
public interface IMapFrom<T>
{
    /// <summary>
    /// Configures the mapping from the source type.
    /// </summary>
    /// <param name="profile">The AutoMapper profile.</param>
    void Mapping(Profile profile) => profile.CreateMap(typeof(T), GetType());
}
