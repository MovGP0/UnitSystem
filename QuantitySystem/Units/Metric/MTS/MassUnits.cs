﻿using QuantitySystem.Quantities.BaseQuantities;
using QuantitySystem.Attributes;

namespace QuantitySystem.Units.Metric.Mts;

[MetricUnit("mt", typeof(Mass<>), true)]
[ReferenceUnit(1000)]
public sealed class MetricTonne : MetricUnit;