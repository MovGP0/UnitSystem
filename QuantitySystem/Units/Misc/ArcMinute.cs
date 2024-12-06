using QuantitySystem.Attributes;
using QuantitySystem.Quantities.DimensionlessQuantities;

namespace QuantitySystem.Units.Misc;

[Unit("arcmin", typeof(Angle<>))]
[ReferenceUnit(1, 60, UnitType = typeof(ArcDegree))]
public sealed class ArcMinute : Unit;