using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using ShInUeXx.Numerics;
using UnitSystem.Dimensions;

namespace UnitSystem.Serialization;

/// <summary>
/// Represents a canonical amount and the exact exponents of its base units.
/// </summary>
/// <typeparam name="T">The type of the amount.</typeparam>
[DataContract]
public struct QuantityInfo<T>
    where T : struct
{
    private ReadOnlyDictionary<string, Rational>? _unit;

    [DataMember(EmitDefaultValue = false)]
    private Dictionary<string, string>? RationalUnit
    {
        get => _unit?.ToDictionary(component => component.Key, component => DimensionExponent.Format(component.Value));
        set
        {
            if (value is not null)
            {
                _unit = new(value.ToDictionary(component => component.Key, component => DimensionExponent.Parse(component.Value)));
            }
        }
    }

    [DataMember(Name = nameof(Unit), EmitDefaultValue = false)]
    private Dictionary<string, float>? LegacyUnit
    {
        get => null;
        set
        {
            if (_unit is null && value is not null)
            {
                _unit = new(value.ToDictionary(component => component.Key, component => DimensionExponent.FromSingle(component.Value)));
            }
        }
    }

    /// <summary>
    /// Initializes quantity information from single-precision unit exponents.
    /// </summary>
    public QuantityInfo(T amount, IDictionary<string, float> unit)
        : this(amount, unit, null)
    {
    }

    /// <summary>
    /// Initializes quantity information from single-precision unit exponents and point or difference semantics.
    /// </summary>
    public QuantityInfo(T amount, IDictionary<string, float> unit, bool? isPoint)
        : this(amount, unit.ToDictionary(component => component.Key, component => DimensionExponent.FromSingle(component.Value)), isPoint)
    {
    }

    /// <summary>
    /// Initializes quantity information from exact unit exponents and optional point or difference semantics.
    /// </summary>
    public QuantityInfo(T amount, IDictionary<string, Rational> unit, bool? isPoint = null)
    {
        ArgumentNullException.ThrowIfNull(unit);
        var components = new Dictionary<string, Rational>(unit);
        if (components.Values.Any(exponent => !Rational.IsFinite(exponent)))
        {
            throw new ArgumentOutOfRangeException(nameof(unit), "Unit exponents must be finite.");
        }

        Amount = amount;
        _unit = new(components);
        IsPoint = isPoint;
    }

    /// <summary>
    /// Gets the canonical amount.
    /// </summary>
    [DataMember]
    public T Amount { get; private set; }

    /// <summary>
    /// Gets the exact exponents of the base units, keyed by symbol.
    /// </summary>
    public ReadOnlyDictionary<string, Rational> Unit
        => _unit ?? throw new InvalidOperationException("Quantity information requires unit components.");

    /// <summary>
    /// Gets whether the amount is a point. Older records use the unit's default when this is null.
    /// </summary>
    [DataMember(EmitDefaultValue = false)]
    public bool? IsPoint { get; private set; }
}
