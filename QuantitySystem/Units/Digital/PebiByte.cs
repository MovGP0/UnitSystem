using QuantitySystem.Attributes;
using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Units.Digital;

[Unit("PiB", typeof(Digital<>))]
[ReferenceUnit(1024, UnitType = typeof(TibiByte))]
public sealed class PebiByte : Unit;