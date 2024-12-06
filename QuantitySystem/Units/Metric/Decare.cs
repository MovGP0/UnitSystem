using QuantitySystem.Attributes;
using QuantitySystem.Quantities;

namespace QuantitySystem.Units.Metric;

/// <summary>
/// Decare by addin Deka to Are
/// </summary>
[MetricUnit("decare", typeof(Area<>))]
[ReferenceUnit(10, UnitType = typeof(Are))]
public sealed class Decare : MetricUnit;