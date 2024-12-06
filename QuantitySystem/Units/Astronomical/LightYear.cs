using QuantitySystem.Attributes;
using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Units.Astronomical;

[MetricUnit("ly", typeof(Length<>))]
[ReferenceUnit(9.460530E+15)]
public sealed class LightYear : MetricUnit;