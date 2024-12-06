using QuantitySystem.Attributes;
using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Units.Digital;

[Unit("kbit", typeof(Digital<>))]
[ReferenceUnit(1000, UnitType = typeof(Bit))]
public sealed class KiloBit : Unit;