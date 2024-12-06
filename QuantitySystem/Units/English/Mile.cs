using QuantitySystem.Attributes;
using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Units.English;

[Unit("mil", typeof(Length<>))]
[ReferenceUnit(1760, UnitType = typeof(Yard))]
public sealed class Mile : Unit;