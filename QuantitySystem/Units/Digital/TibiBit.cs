using QuantitySystem.Attributes;
using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Units.Digital;

[Unit("Tibit", typeof(Digital<>))]
[ReferenceUnit(1024, UnitType = typeof(GibiBit))]
public sealed class TibiBit : Unit;