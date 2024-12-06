using QuantitySystem.Attributes;
using QuantitySystem.Quantities;

namespace QuantitySystem.Units.Metric;

/// <summary>
/// Base unit for Hectare and Decare
/// Hectare: by adding Hecto to Are
/// Decare: by addin Deka to Are
/// </summary>
[MetricUnit("are", typeof(Area<>))]
[ReferenceUnit(100)]
public sealed class Are : MetricUnit;