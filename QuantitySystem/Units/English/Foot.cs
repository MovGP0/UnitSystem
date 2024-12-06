using QuantitySystem.Attributes;
using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Units.English;

[DefaultUnit("ft", typeof(Length<>))]
[ReferenceUnit(0.3048)]
public sealed class Foot : Unit;