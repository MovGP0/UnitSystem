using QuantitySystem.Attributes;
using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Units.Ancient;

[Unit("cubit", typeof(Length<>))]
[ReferenceUnit(0.4572)]
public sealed class Cubit : Unit;