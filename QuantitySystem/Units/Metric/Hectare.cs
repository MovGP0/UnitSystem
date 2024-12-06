using QuantitySystem.Attributes;
using QuantitySystem.Quantities;

namespace QuantitySystem.Units.Metric;

/// <summary>
/// Hectare by adding Hecto to Are
/// </summary>
[MetricUnit("ha", typeof(Area<>))]
[ReferenceUnit(100, UnitType=typeof(Are))]
public sealed class Hectare : MetricUnit;