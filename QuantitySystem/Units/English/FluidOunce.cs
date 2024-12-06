using QuantitySystem.Attributes;
using QuantitySystem.Quantities;

namespace QuantitySystem.Units.English;

[Unit("fl_oz", typeof(Volume<>))]
[ReferenceUnit(1, 20, UnitType = typeof(Pint))]
public sealed class FluidOunce : Unit;