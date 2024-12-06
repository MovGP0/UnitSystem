using QuantitySystem.Attributes;
using QuantitySystem.Quantities.DimensionlessQuantities;

namespace QuantitySystem.Units.Misc;

[Unit("mas", typeof(Angle<>))]
[ReferenceUnit(1, 1000, UnitType = typeof(ArcSecond))]
public sealed class MilliArcSecond : Unit;