using QuantitySystem.Attributes;
using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Units.Misc;

[DefaultUnit("min", typeof(Time<>))]
[ReferenceUnit(60)]
public sealed class Minute : Unit;