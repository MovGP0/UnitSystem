using QuantitySystem.Attributes;
using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Units.Digital;

[Unit("MB", typeof(Digital<>))]
[ReferenceUnit(1000, UnitType = typeof(KiloByte))]
public sealed class MegaByte : Unit;