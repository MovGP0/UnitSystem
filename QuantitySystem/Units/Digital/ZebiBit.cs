using QuantitySystem.Attributes;
using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Units.Digital;

[Unit("Zibit", typeof(Digital<>))]
[ReferenceUnit(1024, UnitType = typeof(ExbiBit))]
public sealed class ZebiBit : Unit;