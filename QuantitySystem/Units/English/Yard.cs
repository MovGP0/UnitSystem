using QuantitySystem.Attributes;
using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Units.English;

[Unit("yd", typeof(Length<>))]
[ReferenceUnit(3, UnitType = typeof(Foot))]
public sealed class Yard : Unit;