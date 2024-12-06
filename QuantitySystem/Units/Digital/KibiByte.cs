using QuantitySystem.Attributes;
using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Units.Digital;

[Unit("KiB", typeof(Digital<>))]
[ReferenceUnit(1024, UnitType = typeof(Byte))]
public sealed class KibiByte : Unit;