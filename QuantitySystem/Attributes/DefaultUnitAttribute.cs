﻿namespace QuantitySystem.Attributes;

/// <summary>
/// Default unit attribute indicates that this is the original value
/// of the unit when creating a quantity of the quantity type.
/// like feet in imperial system
/// also default unit serve as the entry point of the Units Cloud of the Quantity
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public sealed class DefaultUnitAttribute(string symbol, Type quantityType) : UnitAttribute(symbol, quantityType);