using QuantitySystem.Attributes;
using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Units.Digital;

[Unit("Gibit", typeof(Digital<>))]
[ReferenceUnit(1024, UnitType = typeof(MebiBit))]
public sealed class GibiBit : Unit;