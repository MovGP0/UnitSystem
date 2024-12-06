using QuantitySystem.Attributes;
using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Units.Digital;

[Unit("Ebit", typeof(Digital<>))]
[ReferenceUnit(1000, UnitType = typeof(PetaBit))]
public sealed class ExaBit : Unit;