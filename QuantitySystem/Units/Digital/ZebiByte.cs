using QuantitySystem.Attributes;
using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Units.Digital;

[Unit("ZiB", typeof(Digital<>))]
[ReferenceUnit(1024, UnitType = typeof(ExbiByte))]
public sealed class ZebiByte : Unit;