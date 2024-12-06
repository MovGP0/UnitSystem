using QuantitySystem.Attributes;
using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Units.Digital;

[Unit("MiB", typeof(Digital<>))]
[ReferenceUnit(1024, UnitType = typeof(KibiByte))]
public sealed class MebiByte : Unit;