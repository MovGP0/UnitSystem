using QuantitySystem.Attributes;
using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Units.Digital;

[Unit("Yibit", typeof(Digital<>))]
[ReferenceUnit(1024, UnitType = typeof(ZebiBit))]
public sealed class YobiBit : Unit;