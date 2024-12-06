using QuantitySystem.Attributes;
using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Units.English;

[DefaultUnit("lbm", typeof(Mass<>))]
[ReferenceUnit(0.45359237)]
public sealed class Pound : Unit;