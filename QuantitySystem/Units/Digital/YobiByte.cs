using QuantitySystem.Attributes;
using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Units.Digital;

[Unit("YiB", typeof(Digital<>))]
[ReferenceUnit(1024, UnitType = typeof(ZebiByte))]
public sealed class YobiByte : Unit;