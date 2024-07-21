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
    private readonly string _source;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="numerator"></param>
    /// <param name="denominator"></param>
    public ReferenceUnitAttribute(double numerator)
    {
        _numerator = numerator;
        Denominator = 1;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="numerator"></param>
    /// <param name="denominator"></param>
    public ReferenceUnitAttribute(double numerator, double denominator)
    {
        _numerator = numerator;
        Denominator = denominator;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="unitType"></param>
    /// <param name="source">FunctionName.UnitName</param>
    public ReferenceUnitAttribute(Type unitType, string source)
    {
        _source = source;
        UnitType = unitType;
            
        Denominator = 1;

        // the numerator will be calculated based on source value

            
        var sourceFunctionName = source[..source.IndexOf('.')];

        if (!DynamicQuantitySystem.DynamicSourceFunctions.ContainsKey(sourceFunctionName))
        {
            DynamicQuantitySystem.DynamicSourceFunctions[sourceFunctionName] = (u) => 1.0;   // always return 1.0;
        }
    }

    public Type UnitType { get; set; }

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

    public double Times => _numerator / Denominator + Shift;
}