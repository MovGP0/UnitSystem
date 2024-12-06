using QuantitySystem.Attributes;
using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Units.Digital;

[Unit("Kibit", typeof(Digital<>))]
[ReferenceUnit(1024, UnitType = typeof(Bit))]
public sealed class KibiBit : Unit;