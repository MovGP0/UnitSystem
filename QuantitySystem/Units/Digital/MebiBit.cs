using QuantitySystem.Attributes;
using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Units.Digital;

[Unit("Mibit", typeof(Digital<>))]
[ReferenceUnit(1024, UnitType = typeof(KibiBit))]
public sealed class MebiBit : Unit;