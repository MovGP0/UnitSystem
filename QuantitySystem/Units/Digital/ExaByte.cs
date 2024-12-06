using QuantitySystem.Attributes;
using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Units.Digital;

[Unit("EB", typeof(Digital<>))]
[ReferenceUnit(1000, UnitType = typeof(PetaByte))]
public sealed class ExaByte : Unit;