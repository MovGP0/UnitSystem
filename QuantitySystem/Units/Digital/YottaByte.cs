using QuantitySystem.Attributes;
using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Units.Digital;

[Unit("YB", typeof(Digital<>))]
[ReferenceUnit(1000, UnitType = typeof(ZettaByte))]
public sealed class YottaByte : Unit;