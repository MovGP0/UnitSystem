using QuantitySystem.Attributes;
using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Units.Natural;

[MetricUnit("Da", typeof(Mass<>))]
[ReferenceUnit(1.6605388628E-27)]
public sealed class Dalton : MetricUnit;