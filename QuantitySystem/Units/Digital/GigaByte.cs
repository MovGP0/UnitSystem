using QuantitySystem.Attributes;
using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Units.Digital;

[Unit("GB", typeof(Digital<>))]
[ReferenceUnit(1000, UnitType = typeof(MegaByte))]
public sealed class GigaByte : Unit;