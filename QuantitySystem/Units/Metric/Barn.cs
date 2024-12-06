using QuantitySystem.Attributes;
using QuantitySystem.Quantities;

namespace QuantitySystem.Units.Metric;

[MetricUnit("b", typeof(Area<>))]
[ReferenceUnit(1E-28)]
public sealed class Barn : MetricUnit;