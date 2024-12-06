using QuantitySystem.Attributes;
using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Units.Digital;

[Unit("Gbit", typeof(Digital<>))]
[ReferenceUnit(1000, UnitType = typeof(MegaBit))]
public sealed class GigaBit : Unit;