using QuantitySystem.Attributes;
using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Units.Digital;

[Unit("Zbit", typeof(Digital<>))]
[ReferenceUnit(1000, UnitType = typeof(ExaBit))]
public sealed class ZettaBit : Unit;