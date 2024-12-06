using QuantitySystem.Quantities.BaseQuantities;
using QuantitySystem.Attributes;

namespace QuantitySystem.Units.English;

[Unit("slug", typeof(Mass<>))]
[ReferenceUnit(32.17405, UnitType = typeof(Pound))]
public sealed class Slug : Unit;