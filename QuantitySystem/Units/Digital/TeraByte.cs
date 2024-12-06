using QuantitySystem.Attributes;
using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Units.Digital;

[Unit("TB", typeof(Digital<>))]
[ReferenceUnit(1000, UnitType = typeof(GigaByte))]
public sealed class TeraByte : Unit;