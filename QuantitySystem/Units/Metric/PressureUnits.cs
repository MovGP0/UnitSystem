﻿using QuantitySystem.Attributes;
using QuantitySystem.Quantities;

namespace QuantitySystem.Units.Metric;

[MetricUnit("bar", typeof(Pressure<>))]
[ReferenceUnit(1E+5)]
public sealed class Bar : MetricUnit;