using QuantitySystem.Attributes;
using QuantitySystem.Quantities.DimensionlessQuantities;

namespace QuantitySystem.Units.Misc;

[Unit("r", typeof(Angle<>))]
[ReferenceUnit(360, UnitType = typeof(ArcDegree))]
public sealed class Revolution : Unit;