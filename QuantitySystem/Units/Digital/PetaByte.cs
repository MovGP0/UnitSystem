using QuantitySystem.Attributes;
using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Units.Digital;

[Unit("PB", typeof(Digital<>))]
[ReferenceUnit(1000, UnitType = typeof(TeraByte))]
public sealed class PetaByte : Unit;