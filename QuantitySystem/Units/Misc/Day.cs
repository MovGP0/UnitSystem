using QuantitySystem.Attributes;
using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Units.Misc;

[Unit("d", typeof(Time<>))]
[ReferenceUnit(24, UnitType = typeof(Hour))]
public sealed class Day : Unit;