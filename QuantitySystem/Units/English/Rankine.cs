using QuantitySystem.Attributes;
using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Units.English;

[DefaultUnit("R", typeof(Temperature<>))]
[ReferenceUnit(5, 9)]
public sealed class Rankine : Unit;