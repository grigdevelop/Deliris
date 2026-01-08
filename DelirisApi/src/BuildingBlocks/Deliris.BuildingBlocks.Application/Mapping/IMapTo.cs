namespace Deliris.BuildingBlocks.Application.Mapping;

/// <summary>
/// Interface for objects that can be mapped to a destination type.
/// </summary>
/// <typeparam name="T">The destination type to map to.</typeparam>
public interface IMapTo<T>
{
    /// <summary>
    /// Configures the mapping to the destination type.
    /// </summary>
    /// <param name="profile">The AutoMapper profile.</param>
    void Mapping(Profile profile) => profile.CreateMap(GetType(), typeof(T));
}
