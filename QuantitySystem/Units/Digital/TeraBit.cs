using QuantitySystem.Attributes;
using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Units.Digital;

[Unit("Tbit", typeof(Digital<>))]
[ReferenceUnit(1000, UnitType = typeof(GigaBit))]
public sealed class TeraBit : Unit;