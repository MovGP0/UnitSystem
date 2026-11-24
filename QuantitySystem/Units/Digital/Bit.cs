using QuantitySystem.Attributes;
using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Units.Digital;

[DefaultUnit("bit", typeof(Digital<>))]
public sealed class Bit : Unit;
