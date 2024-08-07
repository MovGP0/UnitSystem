﻿using QuantitySystem.Quantities.BaseQuantities;
using QuantitySystem.Attributes;

namespace QuantitySystem.Units.Metric.SI;

[MetricUnit("mol", typeof(AmountOfSubstance<>))]
public sealed class Mole : MetricUnit;