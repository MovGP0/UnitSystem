using QuantitySystem.Attributes;
using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Units.Natural;

[Unit("mp", typeof(Mass<>))]
[ReferenceUnit(1.672621637E-27)]
public sealed class ProtonMass : Unit;