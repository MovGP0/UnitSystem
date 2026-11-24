namespace QuantitySystem.Attributes;

/// <summary>
/// Make a relation between the attributed unit
/// and a parent unit.
/// If UnitType omitted the reference unit will be the default SI unit
/// of the QuantityType of the Unit or DefaultUnit Attribute
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public sealed class ReferenceUnitAttribute : Attribute
{
    private readonly double _numerator;

    private readonly string? _source;

    /// <summary>Defines a reference-unit conversion with a fixed numerator.</summary>
    public ReferenceUnitAttribute(double numerator)
    {
        _numerator = numerator;
        Denominator = 1;
    }

    /// <summary>Defines a reference-unit conversion with a fixed ratio.</summary>
    public ReferenceUnitAttribute(double numerator, double denominator)
    {
        _numerator = numerator;
        Denominator = denominator;
    }

    /// <summary>Defines a conversion factor obtained from a dynamic source.</summary>
    /// <param name="unitType">The reference unit type.</param>
    /// <param name="source">The dynamic conversion source in FunctionName.UnitName form.</param>
    public ReferenceUnitAttribute(Type unitType, string source)
    {
        _source = source;
        UnitType = unitType;

        Denominator = 1;

        // the numerator will be calculated based on source value

        var sourceFunctionName = source[..source.IndexOf('.')];

        if (!DynamicQuantitySystem.DynamicSourceFunctions.ContainsKey(sourceFunctionName))
        {
            DynamicQuantitySystem.DynamicSourceFunctions[sourceFunctionName] = static _ => 1.0;
        }
    }

    public Type? UnitType { get; set; }

    /// <summary>
    /// Shift the conversion factor forward and backward.
    /// </summary>
    public double Shift { get; set; }

    public double Numerator
    {
        get
        {
            if (!string.IsNullOrEmpty(_source))
            {
                var sourceFunctionName = _source[.._source.IndexOf('.')];
                var unitKey = _source[(_source.IndexOf('.') + 1)..];
                return DynamicQuantitySystem.DynamicSourceFunctions[sourceFunctionName](unitKey);
            }

            return _numerator;
        }
    }

    public double Denominator { get; }

    public double Times => (_numerator / Denominator) + Shift;
}
