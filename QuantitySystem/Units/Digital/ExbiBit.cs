using QuantitySystem.Attributes;
using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Units.Digital;

[Unit("Eibit", typeof(Digital<>))]
[ReferenceUnit(1024, UnitType = typeof(PebiBit))]
public sealed class ExbiBit : Unit;