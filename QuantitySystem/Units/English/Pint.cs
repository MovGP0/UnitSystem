using QuantitySystem.Attributes;
using QuantitySystem.Quantities;

namespace QuantitySystem.Units.English;

[DefaultUnit("pt", typeof(Volume<>))]
[ReferenceUnit(0.5682, 1000)]
public sealed class Pint : Unit;