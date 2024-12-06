using QuantitySystem.Attributes;
using QuantitySystem.Quantities;

namespace QuantitySystem.Units.English;

[Unit("gpm", typeof(VolumeFlowRate<>))]
[ReferenceUnit(0.160526349049199, UnitType = typeof(CubicFeetPerMinute))]
public sealed class GallonPerMinute : Unit;