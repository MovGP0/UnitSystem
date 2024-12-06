using QuantitySystem.Attributes;
using QuantitySystem.Quantities;

namespace QuantitySystem.Units.English;

[Unit("lbcal", typeof(Energy<>))]
[ReferenceUnit(1.8, UnitType = typeof(BTU))]
public sealed class PoundCalorie : Unit;