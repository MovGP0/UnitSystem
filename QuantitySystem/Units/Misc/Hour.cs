using QuantitySystem.Attributes;
using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Units.Misc;

[Unit("h", typeof(Time<>))]
[ReferenceUnit(60, UnitType = typeof(Minute))]
public sealed class Hour : Unit;