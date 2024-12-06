using QuantitySystem.Attributes;
using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Units.Digital;

[Unit("Pbit", typeof(Digital<>))]
[ReferenceUnit(1000, UnitType = typeof(TeraBit))]
public sealed class PetaBit : Unit;