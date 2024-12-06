using QuantitySystem.Attributes;
using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Units.Astronomical;

[MetricUnit("pc", typeof(Length<>))]
[ReferenceUnit(3.08568025E+16)]
public sealed class Parsec : MetricUnit;