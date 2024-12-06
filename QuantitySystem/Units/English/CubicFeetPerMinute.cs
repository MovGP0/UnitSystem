using QuantitySystem.Attributes;
using QuantitySystem.Quantities;

namespace QuantitySystem.Units.English;

[DefaultUnit("cfm", typeof(VolumeFlowRate<>))]
[ReferenceUnit(0.0004719474432)]
public sealed class CubicFeetPerMinute : Unit;