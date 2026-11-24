using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace UnitSystem.Serialization;

/// <summary>
/// Represents a canonical amount and the exponents of its base units.
/// </summary>
/// <typeparam name="T">The type of the amount.</typeparam>
[DataContract]
public struct QuantityInfo<T>
    where T : struct
{
    [DataMember(Name = nameof(Unit), IsRequired = true)]
    private Dictionary<string, float>? _unit;

    /// <summary>
    /// Initializes quantity information from a canonical amount and its unit components.
    /// </summary>
    public QuantityInfo(T amount, IDictionary<string, float> unit)
        : this(amount, unit, null)
    {
    }

    /// <summary>
    /// Initializes quantity information with explicit point or difference semantics.
    /// </summary>
    public QuantityInfo(T amount, IDictionary<string, float> unit, bool? isPoint)
    {
        Amount = amount;
        _unit = new Dictionary<string, float>(unit);
        IsPoint = isPoint;
    }

    /// <summary>
    /// Gets the canonical amount.
    /// </summary>
    [DataMember]
    public T Amount { get; private set; }

    /// <summary>
    /// Gets the exponents of the base units, keyed by symbol.
    /// </summary>
    public ReadOnlyDictionary<string, float> Unit
        => new(_unit ?? throw new InvalidOperationException("Quantity information requires unit components."));

    /// <summary>
    /// Gets whether the amount is a point. Older records use the unit's default when this is null.
    /// </summary>
    [DataMember(EmitDefaultValue = false)]
    public bool? IsPoint { get; private set; }
}
