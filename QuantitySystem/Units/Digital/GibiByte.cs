using QuantitySystem.Attributes;
using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Units.Digital;

[Unit("GiB", typeof(Digital<>))]
[ReferenceUnit(1024, UnitType = typeof(MebiByte))]
public sealed class GibiByte : Unit;