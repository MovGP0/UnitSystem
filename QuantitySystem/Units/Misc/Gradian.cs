using QuantitySystem.Attributes;
using QuantitySystem.Quantities.DimensionlessQuantities;

namespace QuantitySystem.Units.Misc;

/// <summary>
/// Decimal degree (1 grad = 0.9°, 400 grad = 360°)
/// </summary>
[Unit("grad", typeof(Angle<>))]
[ReferenceUnit(9, 10, UnitType = typeof(ArcDegree))]
public sealed class Gradian : Unit;