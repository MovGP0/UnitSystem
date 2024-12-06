using QuantitySystem.Quantities.BaseQuantities;
using QuantitySystem.Attributes;

namespace QuantitySystem.Units.English;

[Unit("thou", typeof(Length<>))]
[ReferenceUnit(1.0, 12000.0, UnitType = typeof(Foot))]
public sealed class Thou : Unit;