using QuantitySystem.Attributes;
using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Units.Digital;

[Unit("Ybit", typeof(Digital<>))]
[ReferenceUnit(1000, UnitType = typeof(ZettaBit))]
public sealed class YottaBit : Unit;