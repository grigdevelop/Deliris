using System.Reflection;

namespace Deliris.BuildingBlocks.Domain.Abstractions.Enumerations;

/// <summary>
/// Base class for creating type-safe enumerations in the domain model.
/// Enumerations are immutable value objects that represent a fixed set of named values.
/// </summary>
public abstract class Enumeration : IEquatable<Enumeration>, IComparable<Enumeration>
{
    /// <summary>
    /// Gets the unique identifier of the enumeration value.
    /// </summary>
    public int Id { get; }

    /// <summary>
    /// Gets the name of the enumeration value.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Enumeration"/> class.
    /// </summary>
    /// <param name="id">The unique identifier of the enumeration value.</param>
    /// <param name="name">The name of the enumeration value.</param>
    protected Enumeration(int id, string name)
    {
        Id = id;
        Name = name;
    }

    /// <summary>
    /// Returns a string representation of the enumeration.
    /// </summary>
    /// <returns>The name of the enumeration value.</returns>
    public override string ToString() => Name;

    /// <summary>
    /// Gets all enumeration values of the specified type.
    /// </summary>
    /// <typeparam name="T">The type of enumeration.</typeparam>
    /// <returns>A collection of all enumeration values.</returns>
    public static IEnumerable<T> GetAll<T>() where T : Enumeration
    {
        var fields = typeof(T).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);

        return fields.Select(f => f.GetValue(null)).Cast<T>();
    }

    /// <summary>
    /// Gets an enumeration value by its identifier.
    /// </summary>
    /// <typeparam name="T">The type of enumeration.</typeparam>
    /// <param name="id">The identifier of the enumeration value.</param>
    /// <returns>The enumeration value with the specified identifier.</returns>
    /// <exception cref="InvalidOperationException">Thrown when no enumeration value with the specified identifier is found.</exception>
    public static T FromId<T>(int id) where T : Enumeration
    {
        var matchingItem = Parse<T, int>(id, "id", item => item.Id == id);
        return matchingItem;
    }

    /// <summary>
    /// Gets an enumeration value by its name.
    /// </summary>
    /// <typeparam name="T">The type of enumeration.</typeparam>
    /// <param name="name">The name of the enumeration value.</param>
    /// <returns>The enumeration value with the specified name.</returns>
    /// <exception cref="InvalidOperationException">Thrown when no enumeration value with the specified name is found.</exception>
    public static T FromName<T>(string name) where T : Enumeration
    {
        var matchingItem = Parse<T, string>(name, "name", item => item.Name == name);
        return matchingItem;
    }

    /// <summary>
    /// Tries to get an enumeration value by its identifier.
    /// </summary>
    /// <typeparam name="T">The type of enumeration.</typeparam>
    /// <param name="id">The identifier of the enumeration value.</param>
    /// <param name="result">The enumeration value if found; otherwise, null.</param>
    /// <returns>true if the enumeration value was found; otherwise, false.</returns>
    public static bool TryFromId<T>(int id, out T? result) where T : Enumeration
    {
        result = GetAll<T>().FirstOrDefault(item => item.Id == id);
        return result != null;
    }

    /// <summary>
    /// Tries to get an enumeration value by its name.
    /// </summary>
    /// <typeparam name="T">The type of enumeration.</typeparam>
    /// <param name="name">The name of the enumeration value.</param>
    /// <param name="result">The enumeration value if found; otherwise, null.</param>
    /// <returns>true if the enumeration value was found; otherwise, false.</returns>
    public static bool TryFromName<T>(string name, out T? result) where T : Enumeration
    {
        result = GetAll<T>().FirstOrDefault(item => item.Name == name);
        return result != null;
    }

    /// <summary>
    /// Determines whether the specified object is equal to the current enumeration.
    /// </summary>
    /// <param name="obj">The object to compare with the current enumeration.</param>
    /// <returns>true if the specified object is equal to the current enumeration; otherwise, false.</returns>
    public override bool Equals(object? obj)
    {
        if (obj is not Enumeration otherValue)
        {
            return false;
        }

        var typeMatches = GetType() == obj.GetType();
        var valueMatches = Id.Equals(otherValue.Id);

        return typeMatches && valueMatches;
    }

    /// <summary>
    /// Determines whether the specified enumeration is equal to the current enumeration.
    /// </summary>
    /// <param name="other">The enumeration to compare with the current enumeration.</param>
    /// <returns>true if the specified enumeration is equal to the current enumeration; otherwise, false.</returns>
    public bool Equals(Enumeration? other)
    {
        if (other is null)
        {
            return false;
        }

        var typeMatches = GetType() == other.GetType();
        var valueMatches = Id.Equals(other.Id);

        return typeMatches && valueMatches;
    }

    /// <summary>
    /// Serves as the default hash function.
    /// </summary>
    /// <returns>A hash code for the current enumeration.</returns>
    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    /// <summary>
    /// Compares the current enumeration with another enumeration.
    /// </summary>
    /// <param name="other">The enumeration to compare with the current enumeration.</param>
    /// <returns>A value that indicates the relative order of the objects being compared.</returns>
    public int CompareTo(Enumeration? other)
    {
        return other is null ? 1 : Id.CompareTo(other.Id);
    }

    private static T Parse<T, TValue>(TValue value, string description, Func<T, bool> predicate) where T : Enumeration
    {
        var matchingItem = GetAll<T>().FirstOrDefault(predicate);

        if (matchingItem == null)
        {
            throw new InvalidOperationException($"'{value}' is not a valid {description} in {typeof(T)}");
        }

        return matchingItem;
    }
}
