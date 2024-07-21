using QuantitySystem.Attributes;
using QuantitySystem.Quantities;

namespace QuantitySystem.Units.Misc;

[Unit("rpm", typeof(AngularVelocity<>))]
[ReferenceUnit(2.0 * Math.PI, 60)]
public sealed class RevolutionPerMinute : Unit;