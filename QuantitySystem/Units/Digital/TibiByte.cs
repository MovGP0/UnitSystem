using QuantitySystem.Attributes;
using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Units.Digital;

[Unit("TiB", typeof(Digital<>))]
[ReferenceUnit(1024, UnitType = typeof(GibiByte))]
public sealed class TibiByte : Unit;