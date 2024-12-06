using QuantitySystem.Attributes;
using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Units.Digital;

[Unit("ZB", typeof(Digital<>))]
[ReferenceUnit(1000, UnitType = typeof(ExaByte))]
public sealed class ZettaByte : Unit;