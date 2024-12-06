using QuantitySystem.Attributes;
using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Units.English;

[Unit("st", typeof(Mass<>))]
[ReferenceUnit(14, UnitType = typeof(Pound))]
public sealed class Stone : Unit;