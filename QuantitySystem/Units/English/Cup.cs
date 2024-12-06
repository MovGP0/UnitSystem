using QuantitySystem.Attributes;
using QuantitySystem.Quantities;

namespace QuantitySystem.Units.English;

[Unit("cup", typeof(Volume<>))]
[ReferenceUnit(1, 2, UnitType = typeof(Pint))]
public sealed class Cup : Unit;