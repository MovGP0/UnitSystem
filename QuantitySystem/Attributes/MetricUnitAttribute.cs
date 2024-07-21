using QuantitySystem.Units;

namespace QuantitySystem.Attributes;

/// <summary>
/// Special Attribute for the Metric Units.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public sealed class MetricUnitAttribute : UnitAttribute
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="symbol">Symbol of the unit that appears in display</param>
    /// <param name="quantityType">The CLR type of quantity that this unit is associated to.</param>
    public MetricUnitAttribute(string symbol, Type quantityType)
        : base(symbol, quantityType)
    {
        SystemDefault = false;  // SI units are implicitly defaults even if this field is true.
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="symbol">Symbol of the unit that appears in display</param>
    /// <param name="quantityType">The CLR type of quantity that this unit is associated to.</param>
    /// <param name="systemDefault">Indicates that this unit is default unit of this system in this quantity. {SI namespace not affected}</param>
    public MetricUnitAttribute(string symbol, Type quantityType, bool systemDefault)
        : base(symbol, quantityType)
    {
        SystemDefault = systemDefault;
    }

    public bool SystemDefault { get; }

    // these prefixes take effect when you create the default unit for their system

    /// <summary>
    /// Default prefix for SI if you create the unit with SI
    /// </summary>
    public MetricPrefixes SiPrefix { get; set; } = MetricPrefixes.None;

    /// <summary>
    /// Default prefix for cgs if you create the unit with cgs
    /// </summary>
    public MetricPrefixes CgsPrefix { get; set; } = MetricPrefixes.None;

    /// <summary>
    /// Default prefix for metric if you create the unit with metric
    /// </summary>
    public MetricPrefixes MtsPrefix { get; set; } = MetricPrefixes.None;

    /// <summary>
    /// Default prefix for Gravitational if you create the unit with Gravitational
    /// </summary>
    public MetricPrefixes GravitationalPrefix { get; set; } = MetricPrefixes.None;
}