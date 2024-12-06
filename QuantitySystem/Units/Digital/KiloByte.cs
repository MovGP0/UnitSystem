using QuantitySystem.Attributes;
using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Units.Digital;

[Unit("kB", typeof(Digital<>))]
[ReferenceUnit(1000, UnitType = typeof(Byte))]
public sealed class KiloByte : Unit;