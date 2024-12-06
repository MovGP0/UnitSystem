using QuantitySystem.Attributes;
using QuantitySystem.Quantities.DimensionlessQuantities;

namespace QuantitySystem.Units.Misc;

[Unit("arcsec", typeof(Angle<>))]
[ReferenceUnit(1, 60, UnitType = typeof(ArcMinute))]
public sealed class ArcSecond : Unit;