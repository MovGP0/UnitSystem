using QuantitySystem.Attributes;
using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Units.English;

[Unit("drachm", typeof(Mass<>))]
[ReferenceUnit(1, 256, UnitType = typeof(Pound))]
public sealed class Drachm : Unit;