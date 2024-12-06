using QuantitySystem.Attributes;
using QuantitySystem.Quantities;

namespace QuantitySystem.Units.Metric.Gravitational;

[MetricUnit("gf", typeof(Force<>))]
[ReferenceUnit(1, UnitType = typeof(Pond))]
public sealed class GramForce : MetricUnit;