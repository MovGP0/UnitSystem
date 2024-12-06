using QuantitySystem.Attributes;
using QuantitySystem.Quantities;

namespace QuantitySystem.Units.Metric;

/// <summary>
/// 1 Litre = 1/1000 m^3
/// </summary>
[MetricUnit("L", typeof(Volume<>))]
[ReferenceUnit(1, 1000)]
public sealed class Litre : MetricUnit;