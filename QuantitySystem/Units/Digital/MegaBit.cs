using QuantitySystem.Attributes;
using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Units.Digital;

[Unit("Mbit", typeof(Digital<>))]
[ReferenceUnit(1000, UnitType = typeof(KiloBit))]
public sealed class MegaBit : Unit;