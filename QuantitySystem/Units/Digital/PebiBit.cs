using QuantitySystem.Attributes;
using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Units.Digital;

[Unit("Pibit", typeof(Digital<>))]
[ReferenceUnit(1024, UnitType = typeof(TibiBit))]
public sealed class PebiBit : Unit;